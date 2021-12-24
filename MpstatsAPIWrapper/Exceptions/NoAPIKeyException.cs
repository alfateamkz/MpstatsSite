using System;
using System.Collections.Generic;
using System.Text;

namespace MpstatsAPIWrapper.Exceptions
{
    public class NoAPIKeyException : Exception
    {
        public NoAPIKeyException() : base("Укажите API ключ в свойстве APIKey")
        {
           
        }
    }
}
