using System.Collections.Generic;

namespace ALM_Tenta.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public int CustomerNumber { get; set; }
        public string OrganisationNumber { get; set; }
        public string Name { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<Account> Accounts { get; set; }
    }
}
