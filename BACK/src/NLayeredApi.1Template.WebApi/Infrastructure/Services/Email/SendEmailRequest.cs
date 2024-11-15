using System;
using System.Collections.Generic;
using System.Text;

namespace NaviMente.WebApi.Infrastructure.Services.Email
{
    public class SendEmailRequest
    {
        public string ToEmail { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}