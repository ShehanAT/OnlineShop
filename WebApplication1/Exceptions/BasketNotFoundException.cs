using Microsoft.AspNetCore.Server.IIS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.WebApplication1.Exceptions
{
    public class BasketNotFoundException : Exception
    {
        private string _basketId;
        // 4 overloaded constructors for various different data types 
        public BasketNotFoundException(int basketId) : base($"No basket with ID {basketId} was found")
        {

        }
        public BasketNotFoundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {

        }
        public BasketNotFoundException(string message) : base(message)
        {

        }
        public BasketNotFoundException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
