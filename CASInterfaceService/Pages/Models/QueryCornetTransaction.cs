using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CASInterfaceService.Pages.Models
{
    public class QueryCornetTransaction
    {
        String SearchType;
        public String search_type
        {
            get { return SearchType; }
            set { SearchType = value; }
        }

        String Surname;
        public String surname 
        {
            get { return Surname;  }
            set { Surname = value; } 
        }

        String Given1;
        public String given1
        {
            get { return Given1;  }
            set { Given1 = null; }
        }

        String Given2;
        public String given2
        {
            get { return Given2;  }
            set { Given2 = value; }
        }

        String BirthYear;
        public String birth_year
        {
            get { return BirthYear;  }
            set { BirthYear = value; }
        }

        String BirthYearRange;
        public String birth_year_range
        {
            get { return BirthYearRange; }
            set { BirthYearRange = value; }
        }

        String Gender;
        [MaxLength(1)]
        public String gender
        {
            get { return Gender; }
            set { Gender = value; }
        }

        String IdentifierType;
        public String identifier_type
        {
            get { return IdentifierType; }
            set { IdentifierType = value; }
        }

        String IdentifierText;
        [Required]
        public String identifier_text
        {
            get { return IdentifierText; }
            set { IdentifierText = value; }
        }

        String UserName;
        [Required]
        public String username
        {
            get { return UserName; }
            set { UserName = value; }
        }

        String FullName;
        [Required]
        public String fullname
        {
            get { return FullName; }
            set { FullName = value; }
        }

        String Client;
        public String client
        {
            get { return Client; }
            set { Client = value; }
        }
    }
}
