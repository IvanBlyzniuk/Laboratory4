using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory4.Exceptions
{
    internal class NegativeAgeException : Exception
    {
        public NegativeAgeException(string message) : base(message) { }
    }
}
