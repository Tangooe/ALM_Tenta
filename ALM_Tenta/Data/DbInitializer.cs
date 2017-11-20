using System.Collections.Generic;
using System.IO;
using System.Linq;
using ALM_Tenta.Dtos;
using ALM_Tenta.Models;
using Microsoft.AspNetCore.Hosting;

namespace ALM_Tenta.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context, string path)
        {
            if (context.Customers.Any())
                return;

            var bankData = ReadBankDataFromFile(path);

            context.Customers.AddRange(bankData.Customers);
            context.Accounts.AddRange(bankData.Accounts);
            context.SaveChanges();
        }

        private static BankDataDto ReadBankDataFromFile(string path)
        {
            var lines = File.ReadAllLines(path);
            var customers = new List<Customer>();
            var accounts = new List<Account>();

            var numberOfCustomers = int.Parse(lines[0]);

            for (var i = 1; i <= numberOfCustomers; i++)
                customers.Add(ParseToCustomer(lines[i].Split(";")));

            for (var i = numberOfCustomers + 2; i < lines.Length; i++)
                accounts.Add(ParseToAccount(lines[i].Split(";")));

            return new BankDataDto
            {
                Accounts = accounts,
                Customers = customers
            };
        }

        private static Account ParseToAccount(IReadOnlyList<string> fields)
        {
            return new Account
            {
                AccountNumber = int.Parse(fields[0]),
                CustomerId = int.Parse(fields[1]),
                Balance = decimal.Parse(fields[2])
            };
        }

        private static Customer ParseToCustomer(IReadOnlyList<string> fields)
        {
            return new Customer
            {
                Id = int.Parse(fields[0]),
                OrganisationNumber = fields[1],
                Name = fields[2],
                StreetAddress = fields[3],
                City = fields[4],
                State = fields[5],
                PostalCode = fields[6],
                Country = fields[7],
                PhoneNumber = fields[8]
            };
        }
    }
}