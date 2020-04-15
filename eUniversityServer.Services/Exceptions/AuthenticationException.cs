using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace eUniversityServer.Services.Exceptions
{
    public class AuthenticationException : ServiceException
    {
        public AuthenticationException() : base()
        { }

        public AuthenticationException(string message) : base(message)
        { }

        public AuthenticationException(string message, params object[] args)
        : base(string.Format(CultureInfo.CurrentCulture, message, args))
        { }

        public AuthenticationException(string message, Exception innerException) : base(message, innerException)
        { }

        protected AuthenticationException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}
