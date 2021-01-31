using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETunderthehood.Services
{
    public class EmailMessageSender : IMessageSender
    {
        public string Send()
        {
            return "Send via email.";
        }
    }
}
