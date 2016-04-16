using System;
using System.Management;
using System.Runtime.Serialization;

namespace DriveDetector.Exceptions
{
    [Serializable]
    public class UndefinedEventTypeException : Exception
    {
        public UndefinedEventTypeException() { }

        public UndefinedEventTypeException(string message) : base(message) { }

        public UndefinedEventTypeException(string message, Exception inner) : base(message, inner) { }

        protected UndefinedEventTypeException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
