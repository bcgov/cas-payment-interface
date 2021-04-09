using System;

namespace CASInterfaceService.Pages.Models
{
    public class CornetTransactionRegistrationReply
    {
        String responseCode;
        public String ResponseCode
        {
            get { return responseCode; }
            set { responseCode = value; }
        }

        String responseMessage;
        public String ResponseMessage
        {
            get { return responseMessage; }
            set { responseMessage = value; }
        }
    }
}
