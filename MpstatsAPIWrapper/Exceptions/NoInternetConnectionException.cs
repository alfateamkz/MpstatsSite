using System;
using System.Collections.Generic;
using System.Text;

namespace MpstatsAPIWrapper.Exceptions
{
    public class NoInternetConnectionException : Exception
    {
        public NoInternetConnectionException() : base("Нет подключения к интернету")
        {
           
        }
    }
}
