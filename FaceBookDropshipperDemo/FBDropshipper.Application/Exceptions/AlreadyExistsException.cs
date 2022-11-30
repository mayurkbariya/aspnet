using System;

namespace FBDropshipper.Application.Exceptions
{
    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException(string name) : base($"{name} already exists")
        {
            
        }
        public AlreadyExistsException(string name,string property, object key) : base($"Entity of Type \"{name}\" already exists with {property} ({key})")
        {
            
        }
    }
}