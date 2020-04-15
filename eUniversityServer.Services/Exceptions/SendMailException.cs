using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Runtime.Serialization;
using System.Text;

namespace eUniversityServer.Services.Exceptions
{
    public class SendMailException : ServiceException
    {
        public override HttpStatusCode ErrorCode { get; protected set; } = HttpStatusCode.ServiceUnavailable;

        public SendMailException() : base()
        { }

        public SendMailException(string message) : base(message)
        { }

        public SendMailException(HttpStatusCode code, string message) : base(message)
        { ErrorCode = code; }

        public SendMailException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        { }

        public SendMailException(string message, Exception innerException) : base(message, innerException)
        { }

        protected SendMailException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}
