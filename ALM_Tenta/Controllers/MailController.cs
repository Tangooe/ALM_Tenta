using ALM_Tenta.Serrvices;
using ALM_Tenta.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ALM_Tenta.Controllers
{
    public class MailController : Controller
    {
        private readonly IEmailService _mailService;

        public MailController(IEmailService mailService)
        {
            _mailService = mailService;
        }

        public IActionResult SendMail(MailViewModel model)
        {
            if(model.Recipient.ToLower().EndsWith("nackademin.se"))
            {
                _mailService.SendMail(model.Sender, model.Recipient, model.Topic, model.Message);

                TempData["Message"] = $"a mail has been sent to {model.Recipient}";
                return RedirectToAction("Index", "Customers");
            }

            TempData["Message"] = $"You are not authorized to send to that address";
            return RedirectToAction("Index", "Customers");
        }
    }
}
