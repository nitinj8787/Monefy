using System;
using System.Collections.Generic;
using System.Text;

namespace Monefy.Api.Common.Exceptions
{
    public class UnAuthorizedAccessException : Exception
    {
        public UnAuthorizedAccessException(string message) : base(message)
        {
        }
    }
}
