using ALM_Tenta.Models;
using System.Collections.Generic;

namespace ALM_Tenta.Dtos
{
    public class BankDataDto
    {
        public IList<Customer> Customers { get; set; }
        public IList<Account> Accounts { get; set; }
    }
}
