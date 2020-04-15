using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace eUniversityServer.Services.Exceptions
{
    public class RegistrationException : ServiceException
    {
        public RegistrationException() : base()
        { }

        public RegistrationException(string message) : base(message)
        { }

        public RegistrationException(string message, params object[] args)
        : base(string.Format(CultureInfo.CurrentCulture, message, args))
        { }

        public RegistrationException(string message, Exception innerException) : base(message, innerException)
        { }

        protected RegistrationException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}
