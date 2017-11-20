using Microsoft.AspNetCore.Mvc.Rendering;

namespace ALM_Tenta.ViewModels
{
    public class TransferViewModel
    {
        public int SenderAccountId { get; set; }
        public SelectList SenderAccounts { get; set; }

        public int ReciepentAccountId { get; set; }
        public SelectList ReciepentAccounts { get; set; }

        public decimal Amount { get; set; }
    }
}