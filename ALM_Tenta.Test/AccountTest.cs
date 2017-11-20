using ALM_Tenta.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ALM_Tenta.Test
{
    [TestClass]
    public class AccountTest
    {
        private Account _account1001;
        private Account _account1002;

        [TestInitialize]
        public void Initialize()
        {
            _account1001 = new Account
            {
                AccountNumber = 1001,
                Balance = 5000,
                CustomerId = 1
            };
            _account1002 = new Account
            {
                AccountNumber = 1002,
                Balance = 5000,
                CustomerId = 1
            };
        }

        [TestMethod]
        public void CanDoWithDrawalsFromAccount()
        {
            Assert.IsTrue(_account1001.Withdrawal(2000));
            Assert.AreEqual(_account1001.Balance, 3000);
        }

        [TestMethod]
        public void WithdrawalsFromAccount_WithInsufficientFunds_ShouldReturnFalse()
        {
            Assert.IsFalse(_account1001.Withdrawal(100000));
        }

        [TestMethod]
        public void CanDoDepositsToAccount()
        {
            _account1002.Deposit(2000);
            Assert.AreEqual(_account1002.Balance, 7000);
        }
    }
}