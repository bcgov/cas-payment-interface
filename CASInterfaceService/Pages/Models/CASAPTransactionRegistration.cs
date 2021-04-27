using System.Collections.Generic;

namespace CASInterfaceService.Pages.Models
{
    public class CASAPTransactionRegistration
    {
        List<CASAPTransaction> casAPTransactionList;
        static CASAPTransactionRegistration casregd = null;

        private CASAPTransactionRegistration()
        {
            casAPTransactionList = new List<CASAPTransaction>();
        }

        public static CASAPTransactionRegistration getInstance()
        {
            if (casregd == null)
            {
                casregd = new CASAPTransactionRegistration();
                return casregd;
            }
            else
            {
                return casregd;
            }
        }

        public void Add(CASAPTransaction casapTransaction)
        {
            casAPTransactionList.Add(casapTransaction);
        }

        public List<CASAPTransaction> getAllCASAPTransaction()
        {
            return casAPTransactionList;
        }

        public List<CASAPTransaction> getUpdateCASAPTransaction()
        {
            return casAPTransactionList;
        }
    }
}
