using Application.Contracts.DTO;
using Application.Contracts.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
	public class EmailSenderService : IEmailSenderService
	{
		private readonly IConfiguration _configuration;

		public EmailSenderService(IConfiguration config)
		{
			_configuration = config;
		}
		public async Task SendEmailAsync(string fromAddress, string toAddress, string subject, string message)
		{
			var senderEmail = _configuration["ReturnPaths:SenderEmail"];
			var mailMessage = new MailMessage(fromAddress, toAddress, subject, message);
			using (var client = new SmtpClient("mail.vizalys.com", int.Parse("587"))
			{
				Credentials = new NetworkCredential(senderEmail, "vizReply01#")
			})
			{
				await client.SendMailAsync(mailMessage);
			}
		}

		public async Task SendContactUsEmail(ContactUsDto data)
		{
			var senderEmail = _configuration["ReturnPaths:SenderEmail"];
			
			string message = $"<p>Name: {data.Name}</p>" +
				$"<p>Phone: {data.Phone}</p>" +
				$"<p>Email: {data.Email}</p>" +
				$"<p>Organization Name: {data.Organization}</p>" +
				$"<p>Designation: {data.Designation}</p>" +
				$"<p>Number Of Employees: {data.NoOfEmployees}</p>" +
				$"<p>Business Domain: {data.BusinessDomain}</p>" +
				$"<p>How can we help you?: {data.Subject}</p>" +
				$"<p>Message: {data.Query}</p>";

			var mailMessage = new MailMessage(senderEmail, "hamza.gaya@ratechnologies.net", "Contact us query", message);
			mailMessage.IsBodyHtml = true;
			using (var client = new SmtpClient("mail.vizalys.com", int.Parse("587"))
			{
				Credentials = new NetworkCredential(senderEmail, "vizReply01#")
			})
			{
				await client.SendMailAsync(mailMessage);
			}
		}
	}
}
