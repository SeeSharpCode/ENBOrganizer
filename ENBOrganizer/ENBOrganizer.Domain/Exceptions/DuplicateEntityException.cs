using System;

namespace ENBOrganizer.Domain.Exceptions
{
    public class DuplicateEntityException : Exception
    {
        public DuplicateEntityException(string message): base(message) { }
    }
}
