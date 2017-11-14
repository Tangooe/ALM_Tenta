namespace ALM_Tenta.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public int SenderId { get; set; }
        public Account Sender { get; set; }

        public int RecipentId { get; set; }
        public Account Recipient { get; set; }

        public decimal Amount { get; set; }
    }
}