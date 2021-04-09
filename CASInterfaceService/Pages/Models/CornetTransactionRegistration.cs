using Microsoft.AspNetCore.Authentication.Twitter;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CASInterfaceService.Pages.Models
{
    public class CornetTransactionRegistration
    {
        List<CornetTransaction> cornetTransactionList;
        static CornetTransactionRegistration casregd = null;

        private CornetTransactionRegistration()
        {
            cornetTransactionList = new List<CornetTransaction>();
        }

        public static CornetTransactionRegistration getInstance()
        {
            if (casregd == null)
            {
                casregd = new CornetTransactionRegistration();
                return casregd;
            }
            else
            {
                return casregd;
            }
        }

        public void Add(CornetTransaction cornetTransaction)
        {
            cornetTransactionList.Add(cornetTransaction);
        }

        public List<CornetTransaction> getAllCornetTransaction()
        {
            return cornetTransactionList;
        }

        public List<CornetTransaction> getUpdateCornetTransaction()
        {
            return cornetTransactionList;
        }
    }
}
