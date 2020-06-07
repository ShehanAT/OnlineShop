using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WebApplication1.Interfaces;

namespace Microsoft.WebApplication1.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string messsage)
        {

            // TODO: wire this to a email sending service(SendGrid), say task is completed for now
            return Task.CompletedTask;
        }
    }
}
