using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Runtime.Serialization;
using System.Text;

namespace eUniversityServer.Services.Exceptions
{
    public class CanceledException : ServiceException
    {
        public override HttpStatusCode ErrorCode { get; protected set; } = HttpStatusCode.BadRequest;

        public CanceledException() : base()
        { }

        public CanceledException(string message) : base(message)
        { }

        public CanceledException(HttpStatusCode code, string message) : base(message)
        { ErrorCode = code; }

        public CanceledException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        { }

        public CanceledException(string message, Exception innerException) : base(message, innerException)
        { }

        protected CanceledException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}
