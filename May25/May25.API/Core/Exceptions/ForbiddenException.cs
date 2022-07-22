using System;
using System.Runtime.Serialization;

namespace May25.API.Core.Exceptions
{
    [Serializable]
    public class ForbiddenException : Exception
    {
        public ForbiddenException()
        {

        }

        public ForbiddenException(string message) : base(message)
        {

        }

        public ForbiddenException(string message, Exception inner) : base(message, inner)
        {

        }

        protected ForbiddenException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
