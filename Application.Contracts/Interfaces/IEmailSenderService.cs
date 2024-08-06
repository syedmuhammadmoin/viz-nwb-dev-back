using Application.Contracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Interfaces
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(string fromAddress, string toAddress, string subject, string message);
        Task SendContactUsEmail(ContactUsDto data);
    }
}
