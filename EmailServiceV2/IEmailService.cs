using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailServiceV2
{
    public interface IEmailService
    {
        void SendEmail(Message message);
    }
}
