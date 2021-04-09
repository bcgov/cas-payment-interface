using System;
using System.ComponentModel.DataAnnotations;

namespace CASInterfaceService.Pages.Models
{
    // CBCORVSC-64: New data model for Cornet Transactions - Event Data

    public class CornetTransactionEventData
    {
        String DataElementNM;
        [Required]
        public String data_element_nm
        {
            get { return DataElementNM; }
            set { DataElementNM = value; }
        }

        String DataValueTXT;
        [Required]
        public String data_value_txt
        {
            get { return DataValueTXT; }
            set { DataValueTXT = value; }
        }
    }
}
