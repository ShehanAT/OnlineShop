using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.WebApplication1.Interfaces;
namespace Microsoft.WebApplication1.Extentions
{
    public static class EmailConfirmationExtentions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string confirmationLink)
        { /* first param this IEmailSender is the implementation of IEmailSender which is passed as a parameter to this method*/
            return emailSender.SendEmailAsync(email, "Confirm Your Email", $"This email was sent to confirm your account registered at WebApplication1 ASP.NET application. Please confirm your account by clicking this link: `{HtmlEncoder.Default.Encode(confirmationLink)}`");
        }
  
    }
}
