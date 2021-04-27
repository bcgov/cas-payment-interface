using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace CASInterfaceService
{
    public class Program
    {
        private const string URL = "https://<server>:<port>ords/cas/cfs/apinvoice/";
        private const string TokenURL = "https://<server>:<port>/ords/casords/oauth/token";

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public void CallCAS()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenURL);

            WebRequest request = WebRequest.Create("http://www.temp.com/?param1=x&param2=y");
            request.Method = "GET";
            WebResponse response = request.GetResponse();

            client.Dispose();
        }
    }
}
