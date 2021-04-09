using Gov.Cscp.VictimServices.Public.JsonObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CASInterfaceService.Pages.Models
{
    public class CornetDynamicsReply
    {
        public String odataContext;
        [Required]
        public String odatacontext
        {
            get { return odataContext; }
            set { odataContext = value; }
        }

        bool isSuccess;
        [Required]
        public bool IsSuccess
        {
            get { return isSuccess; }
            set { isSuccess = value; }
        }

        String result;
        [Required]
        public String Result
        {
            get { return result; }
            set { result = value; }
        }
    }
}
