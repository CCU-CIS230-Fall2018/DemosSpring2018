using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RegexAndExceptionHandling
{
    [Serializable]
    public class ArgumentNullOrWhitespaceException : ArgumentException
    {
        public ArgumentNullOrWhitespaceException()
        {
        }

        public ArgumentNullOrWhitespaceException(string message)
            : base(message)
        {
        }

        public ArgumentNullOrWhitespaceException(string message, string paramName)
            : base(message, paramName)
        {
        }

        public ArgumentNullOrWhitespaceException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected ArgumentNullOrWhitespaceException(SerializationInfo info, StreamingContext context)
        {

        }
    }
}
