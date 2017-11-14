using System.Collections.Generic;

namespace ALM_Tenta.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public int CustomerNumber { get; set; }
        public int OrganisationNumber { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public IList<Account> Accounts { get; set; }
    }
}
