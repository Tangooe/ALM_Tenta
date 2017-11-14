﻿using System.Collections.Generic;

namespace ALM_Tenta.Models
{
    public class Account
    {
        public int Id { get; set; }
        public int AccountNumber { get; set; }
        public decimal Balance { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public ICollection<Transaction> SenderTransactions { get; set; }
        public ICollection<Transaction> RecipentTransactions { get; set; }
    }
}