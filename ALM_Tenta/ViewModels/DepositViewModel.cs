using Microsoft.AspNetCore.Mvc.Rendering;

namespace ALM_Tenta.ViewModels
{
    public class DepositViewModel
    {
        public int Id { get; set; }
        public SelectList Accounts { get; set; }
        public decimal Amount { get; set; }
    }
}
