using System;

namespace Sushi.Room.Domain.Exceptions
{
    public class SushiRoomDomainException : Exception
    {
        public SushiRoomDomainException()
        {

        }

        public SushiRoomDomainException(string message) : base(message)
        {

        }

        public SushiRoomDomainException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
