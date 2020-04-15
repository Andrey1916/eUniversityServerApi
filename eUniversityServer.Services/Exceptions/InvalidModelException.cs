using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace eUniversityServer.Services.Exceptions
{
    public class InvalidModelException : ServiceException
    {
        public InvalidModelException() : base()
        { }

        public InvalidModelException(string message) : base(message)
        { }

        public InvalidModelException(string message, params object[] args)
        : base(string.Format(CultureInfo.CurrentCulture, message, args))
        { }

        public InvalidModelException(string message, Exception innerException) : base(message, innerException)
        { }

        protected InvalidModelException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}
