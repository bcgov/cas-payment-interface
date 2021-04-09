using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CASInterfaceService.Pages.Models;
using Gov.Cscp.VictimServices.Public.JsonObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CASInterfaceService.Pages.Models.Extensions;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CASInterfaceService.Pages.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CornetTransactionController : Controller
    {
        private string URL = "";
        private string TokenURL = "";
        private string clientID = "";
        private string secret = "";

        private readonly IConfiguration _configuration;
        //private readonly IHttpContextAccessor _httpContextAccessor;

        public CornetTransactionController(IConfiguration configuration)//, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            //_httpContextAccessor = httpContextAccessor;
        }


        // POST: api/<controller>
        [HttpPost]
        public CornetTransactionRegistrationReply RegisterCornetTransaction(CornetTransaction cornetTransaction)
        {
            Console.WriteLine(DateTime.Now + " In RegisterCornetTransaction");
            CornetTransactionRegistrationReply cornetregreply = new CornetTransactionRegistrationReply();
            CornetTransactionRegistration.getInstance().Add(cornetTransaction);
            Console.WriteLine(DateTime.Now + " Received data from Cornet");

            var t = Task.Run(() => CallDynamicsWithCornetData(_configuration, cornetTransaction));
            t.Wait();
            Console.WriteLine(DateTime.Now + " Sent data to Dynamics");

            if (t.Result.Contains("Cornet Notification "))
            {
                cornetregreply.ResponseCode = "200";
                cornetregreply.ResponseMessage = "Success";
                Console.WriteLine(DateTime.Now + " Response Success");
            }
            else
            {
                //JObject tempJson = JObject.Parse(t.Result);
                //CornetDynamicsReply replyJson = new CornetDynamicsReply();

                //if (t.IsCompletedSuccessfully == true)
                //{
                //    cornetregreply.ResponseMessage = "Success";
                //    cornetregreply.ResponseCode = null;// t.Result;
                //    Console.WriteLine(DateTime.Now + " Response Success");
                //}
                //else
                //{
                    cornetregreply.ResponseMessage = "Failure";
                    cornetregreply.ResponseCode = t.Result;
                    Console.WriteLine(DateTime.Now + " Response Fail");
                //}
            }

            // Responses as follows:
            // 200 - Status OK - Automatically Done
            // 400 - Bad Request (Malformed JSON) - Automatically Done
            // 500 - Internal Server Error (Something wrong on our end)
            // 201 - If anything is being created on our end based on the notification sent
            // This next line is just a sample of how to do it:
            //this.HttpContext.Response.StatusCode = 444;

            Console.WriteLine(DateTime.Now + " Exit RegisterCornetTransaction");
            return cornetregreply;

        }

        private static async Task<string> CallDynamicsWithCornetData(IConfiguration configuration, CornetTransaction model)
        {
            Console.WriteLine(DateTime.Now + " In CallDynamicsWithCornetData");
            HttpClient httpClient = null;
            try
            {
                var cornetData = model.ToCornetDynamicsModel();
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                var cornetJson = JsonConvert.SerializeObject(cornetData, settings);
                cornetJson = cornetJson.Replace("odatatype", "@odata.type");

                // Get results into the tuple
                var endpointAction = "vsd_CreateCORNETNotifications";
                Console.WriteLine(DateTime.Now + " Set endpoint " + endpointAction);
                var tuple = await GetDynamicsHttpClientNew(configuration, cornetJson, endpointAction);
                Console.WriteLine(DateTime.Now + " Got result from Dynamics");

                string tempResult = tuple.Item1.ToString();

                string tempJson = tuple.Item3.ToString();
                tempJson = tempJson.Replace("@odata.context", "oDataContext");

                DynamicsResponseModel deserializeJson = JsonConvert.DeserializeObject<DynamicsResponseModel>(tempJson);

                DynamicsResponse dynamicsResponse = new DynamicsResponse();

                dynamicsResponse.IsSuccess = deserializeJson.IsSuccess;
                dynamicsResponse.Result = deserializeJson.Result;

                if (dynamicsResponse.Result == null)
                {
                    dynamicsResponse.odatacontext = tempJson;
                }
                else
                {
                    dynamicsResponse.odatacontext = dynamicsResponse.Result;
                }

                Console.WriteLine(DateTime.Now + " Return results from Dynamics");
                return dynamicsResponse.odatacontext;

            }
            finally
            {
                if (httpClient != null)
                    httpClient.Dispose();
            }
        }

        static async Task<Tuple<int, HttpResponseMessage, string>> GetDynamicsHttpClientNew(IConfiguration configuration, String model, String endPointName)
        {
            Console.WriteLine(DateTime.Now + " In GetDynamicsHttpClientNew");
            var builder = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddUserSecrets<Program>(); // must also define a project guid for secrets in the .cspro – add tag <UserSecretsId> containing a guid
            var Configuration = builder.Build();
            Console.WriteLine(DateTime.Now + " Build Configuration");

            string dynamicsOdataUri = Configuration["DYNAMICS_ODATA_URI"]; // Dynamics ODATA endpoint
            string dynamicsJobName = endPointName;// Configuration["DYNAMICS_JOB_NAME"]; // Dynamics Job Name

            if (string.IsNullOrEmpty(dynamicsOdataUri))
            {
                throw new Exception("Configuration setting DYNAMICS_ODATA_URI is blank.");
            }

            // Cloud - x.dynamics.com
            string aadTenantId = Configuration["DYNAMICS_AAD_TENANT_ID"]; // Cloud AAD Tenant ID
            string serverAppIdUri = Configuration["DYNAMICS_SERVER_APP_ID_URI"]; // Cloud Server App ID URI
            string appRegistrationClientKey = Configuration["DYNAMICS_APP_REG_CLIENT_KEY"]; // Cloud App Registration Client Key
            string appRegistrationClientId = Configuration["DYNAMICS_APP_REG_CLIENT_ID"]; // Cloud App Registration Client Id

            // One Premise ADFS (2016)
            string adfsOauth2Uri = Configuration["ADFS_OAUTH2_URI"]; // ADFS OAUTH2 URI - usually /adfs/oauth2/token on STS
            string applicationGroupResource = Configuration["DYNAMICS_APP_GROUP_RESOURCE"]; // ADFS 2016 Application Group resource (URI)
            string applicationGroupClientId = Configuration["DYNAMICS_APP_GROUP_CLIENT_ID"]; // ADFS 2016 Application Group Client ID
            string applicationGroupSecret = Configuration["DYNAMICS_APP_GROUP_SECRET"]; // ADFS 2016 Application Group Secret
            string serviceAccountUsername = Configuration["DYNAMICS_USERNAME"]; // Service account username
            string serviceAccountPassword = Configuration["DYNAMICS_PASSWORD"]; // Service account password

            // API Gateway to NTLM user.  This is used in v8 environments.  Note that the SSG Username and password are not the same as the NTLM user.
            string ssgUsername = Configuration["SSG_USERNAME"];  // BASIC authentication username
            string ssgPassword = Configuration["SSG_PASSWORD"];  // BASIC authentication password
            Console.WriteLine(DateTime.Now + " Variables have been set");

            ServiceClientCredentials serviceClientCredentials = null;
            if (!string.IsNullOrEmpty(appRegistrationClientId) && !string.IsNullOrEmpty(appRegistrationClientKey) && !string.IsNullOrEmpty(serverAppIdUri) && !string.IsNullOrEmpty(aadTenantId))
            // Cloud authentication - using an App Registration's client ID, client key.  Add the App Registration to Dynamics as an Application User.
            {
                Console.WriteLine(DateTime.Now + " Trying Cloud Authentication");
                var authenticationContext = new AuthenticationContext(
                "https://login.windows.net/" + aadTenantId);
                ClientCredential clientCredential = new ClientCredential(appRegistrationClientId, appRegistrationClientKey);
                var task = authenticationContext.AcquireTokenAsync(serverAppIdUri, clientCredential);
                task.Wait();
                var authenticationResult = task.Result;
                string token = authenticationResult.CreateAuthorizationHeader().Substring("Bearer ".Length);
                serviceClientCredentials = new TokenCredentials(token);
            }
            if (!string.IsNullOrEmpty(adfsOauth2Uri) &&
                        !string.IsNullOrEmpty(applicationGroupResource) &&
                        !string.IsNullOrEmpty(applicationGroupClientId) &&
                        !string.IsNullOrEmpty(applicationGroupSecret) &&
                        !string.IsNullOrEmpty(serviceAccountUsername) &&
                        !string.IsNullOrEmpty(serviceAccountPassword))
            // ADFS 2016 authentication - using an Application Group Client ID and Secret, plus service account credentials.
            {
                Console.WriteLine(DateTime.Now + " Trying ADFS Authentication");
                // create a new HTTP client that is just used to get a token.
                var stsClient = new HttpClient();

                //stsClient.DefaultRequestHeaders.Add("x-client-SKU", "PCL.CoreCLR");
                //stsClient.DefaultRequestHeaders.Add("x-client-Ver", "5.1.0.0");
                //stsClient.DefaultRequestHeaders.Add("x-ms-PKeyAuth", "1.0");

                stsClient.DefaultRequestHeaders.Add("client-request-id", Guid.NewGuid().ToString());
                stsClient.DefaultRequestHeaders.Add("return-client-request-id", "true");
                stsClient.DefaultRequestHeaders.Add("Accept", "application/json");

                // Construct the body of the request
                var pairs = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("resource", applicationGroupResource),
                    new KeyValuePair<string, string>("client_id", applicationGroupClientId),
                    new KeyValuePair<string, string>("client_secret", applicationGroupSecret),
                    new KeyValuePair<string, string>("username", serviceAccountUsername),
                    new KeyValuePair<string, string>("password", serviceAccountPassword),
                    new KeyValuePair<string, string>("scope", "openid"),
                    new KeyValuePair<string, string>("response_mode", "form_post"),
                    new KeyValuePair<string, string>("grant_type", "password")
                 };

                Console.WriteLine(DateTime.Now + " Set ADFS variables and headers");

                // This will also set the content type of the request
                var content = new FormUrlEncodedContent(pairs);
                Console.WriteLine(DateTime.Now + " content: " + content);
                // send the request to the ADFS server
                Console.WriteLine(DateTime.Now + " About to send request to ADFS");
                var _httpResponse = stsClient.PostAsync(adfsOauth2Uri, content).GetAwaiter().GetResult();
                var _responseContent = _httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                // response should be in JSON format.
                try
                {
                    Dictionary<string, string> result = JsonConvert.DeserializeObject<Dictionary<string, string>>(_responseContent);
                    string token = result["access_token"];
                    Console.WriteLine(DateTime.Now + " Got a token");
                    // set the bearer token.
                    serviceClientCredentials = new TokenCredentials(token);


                    // Code to perform Scheduled task
                    var client = new HttpClient();
                    client.DefaultRequestHeaders.Add("x-client-SKU", "PCL.CoreCLR");
                    client.DefaultRequestHeaders.Add("x-client-Ver", "5.1.0.0");
                    client.DefaultRequestHeaders.Add("x-ms-PKeyAuth", "1.0");
                    client.DefaultRequestHeaders.Add("client-request-id", Guid.NewGuid().ToString());
                    client.DefaultRequestHeaders.Add("return-client-request-id", "true");
                    client.DefaultRequestHeaders.Add("Accept", "application/json");

                    client = new HttpClient();
                    var Authorization = $"Bearer {token}";
                    client.DefaultRequestHeaders.Add("Authorization", Authorization);
                    client.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                    client.DefaultRequestHeaders.Add("OData-Version", "4.0");
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                    //client.DefaultRequestHeaders.Add("content-type", "application/json");
                    //client.DefaultRequestHeaders.Add("Content-Type", "application/json; charset=utf-8");

                    string url = dynamicsOdataUri + dynamicsJobName;
                    Console.WriteLine(DateTime.Now + " Set full URL to speak to Dynamics: " + url);

                    HttpRequestMessage _httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
                    _httpRequest.Content = new StringContent(model, Encoding.UTF8, "application/json");
                    //_httpRequest.Content = new StringContent(System.IO.File.ReadAllText(@"C:\Temp\VSD-RestSampleData3.txt"), Encoding.UTF8, "application/json");
                    Console.WriteLine(DateTime.Now + " Got HTTP Request ready");

                    var _httpResponse2 = await client.SendAsync(_httpRequest);
                    HttpStatusCode _statusCode = _httpResponse2.StatusCode;

                    var _responseString = _httpResponse2.ToString();
                    Console.WriteLine(DateTime.Now + " Got HTTP Response");//: " + _responseString);
                    var _responseContent2 = await _httpResponse2.Content.ReadAsStringAsync();

                    Console.Out.WriteLine(DateTime.Now + " model: " + model);
                    Console.Out.WriteLine(DateTime.Now + " responseString: " + _responseString);
                    Console.Out.WriteLine(DateTime.Now + " responseContent2: " + _responseContent2);

                    Console.WriteLine(DateTime.Now + " Exit GetDynamicsHttpClientNew");
                    return new Tuple<int, HttpResponseMessage, string>((int)_statusCode, _httpResponse2, _responseContent2);
                    // End of scheduled task
                }
                catch (Exception e)
                {
                    return new Tuple<int, HttpResponseMessage, string>(100, null, "Error");
                    throw new Exception(e.Message + " " + _responseContent);
                }

            }
            else if (!string.IsNullOrEmpty(ssgUsername) && !string.IsNullOrEmpty(ssgPassword))
            // Authenticate using BASIC authentication - used for API Gateways with BASIC authentication.  Add the NTLM user associated with the API gateway entry to Dynamics as a user.            
            {
                serviceClientCredentials = new BasicAuthenticationCredentials()
                {
                    UserName = ssgUsername,
                    Password = ssgPassword
                };
            }
            else
            {
                throw new Exception("No configured connection to Dynamics.");
            }

            return new Tuple<int, HttpResponseMessage, string>(100, null, "Error");
        }

        [HttpPost("InsertCornetTransaction")]
        public IActionResult InsertCornetTransaction(CornetTransaction cornetTransaction)
        {
            try
            {
                Console.WriteLine(DateTime.Now + " In InsertCornetTransaction");
                CornetTransactionRegistrationReply casregreply = new CornetTransactionRegistrationReply();
                CornetTransactionRegistration.getInstance().Add(cornetTransaction);
                casregreply.ResponseMessage = "Success";

                return Ok(casregreply);
            }
            catch(Exception e)
            {
                Console.WriteLine(DateTime.Now + " Error in InsertCornetTransaction. " + e.ToString());
                return StatusCode(e.HResult);
            }

        }

        internal class DynamicsResponse
        {
            public string odatacontext { get; set; }
            public bool IsSuccess { get; set; }
            public bool IsCompletedSuccessfully { get; set; }
            public string Result { get; set; }
        }
    }
}
