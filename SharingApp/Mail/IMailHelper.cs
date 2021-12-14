using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharingApp.Mail
{
   public interface IMailHelper
    {
        void sendMail(InputEmailMessage model);
    }
}
