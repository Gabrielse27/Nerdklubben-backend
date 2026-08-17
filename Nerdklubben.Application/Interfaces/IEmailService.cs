using System;
using System.Collections.Generic;
using System.Text;

namespace Nerdklubben.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendConfirmationEmailAsync(string toEmail, string recipientName);
    }
}
