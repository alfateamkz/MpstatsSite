using System;
using System.Collections.Generic;
using System.Text;

namespace MpstatsAPIWrapper.Exceptions
{
    public class APIUnauthorizedException : Exception
    {
        public APIUnauthorizedException() : base("Неверный API ключ")
        {
           
        }
    }
}
