using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace eUniversityServer.Services.Exceptions
{
    public class NotFoundException : ServiceException
    {
        public override HttpStatusCode ErrorCode { get; protected set; } = HttpStatusCode.NotFound;

        public NotFoundException() : base()
        { }

        public NotFoundException(string message) : base(message)
        { }

        public NotFoundException(string message, params object[] args)
        : base(string.Format(CultureInfo.CurrentCulture, message, args))
        { }

        public NotFoundException(string message, Exception innerException) : base(message, innerException)
        { }

        protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}
