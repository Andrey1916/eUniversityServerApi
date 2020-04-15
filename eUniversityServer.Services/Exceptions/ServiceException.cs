using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace eUniversityServer.Services.Exceptions
{
    public class ServiceException : Exception
    {
        public virtual HttpStatusCode ErrorCode { get; protected set; } = HttpStatusCode.BadRequest;

        public ServiceException() : base()
        { }

        public ServiceException(string message) : base(message)
        { }

        public ServiceException(HttpStatusCode code, string message) : base(message)
        { ErrorCode = code; }

        public ServiceException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        { }

        public ServiceException(string message, Exception innerException) : base(message, innerException)
        { }

        protected ServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}
