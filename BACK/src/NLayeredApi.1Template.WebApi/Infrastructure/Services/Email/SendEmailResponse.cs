using System;
using System.Collections.Generic;
using System.Text;

namespace NaviMente.WebApi.Infrastructure.Services.Email
{
    public class SendEmailResponse
    {
        public bool Sent { get; private set; }
        public string? Error { get; private set; }

        public static SendEmailResponse Ok()
        {
            return new SendEmailResponse()
            {
                Sent = true,
                Error = null
            };
        }

        public static SendEmailResponse Failed(string error)
        {
            return new SendEmailResponse()
            {
                Sent = false,
                Error = error
            };
        }

    }
}