using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CASInterfaceService.Pages.Models
{
    public class CASAPQuery
    {
        String InvoiceNumber;
        [Required]
        [MaxLength(40)]
        public String invoiceNumber
        {
            get { return InvoiceNumber; }
            set { InvoiceNumber = value; }
        }

        String SupplierNumber;
        [Required]
        [MaxLength(40)]
        public String supplierNumber
        {
            get { return SupplierNumber; }
            set { SupplierNumber = value; }
        }

        String SupplierSiteNumber;
        [Required]
        [MaxLength(40)]
        public String supplierSiteNumber
        {
            get { return SupplierSiteNumber; }
            set { SupplierSiteNumber = value; }
        }

        //String invoiceStatus;
        //public string InvoiceStatus
        //{
        //    get { return invoiceStatus; }
        //    set { invoiceStatus = value; }
        //}

        //String paymentStatus;
        //public string PaymentStatus
        //{
        //    get { return paymentStatus; }
        //    set { paymentStatus = value; }
        //}

        //Double paymentNumber;
        //public Double PaymentNumber
        //{
        //    get { return paymentNumber;  }
        //    set { paymentNumber = value; }
        //}

        //DateTime paymentDate;
        //public DateTime PaymentDate
        //{
        //    get { return paymentDate; }
        //    set { paymentDate = value; }
        //}
    }
}
