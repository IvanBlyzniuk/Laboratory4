using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory4.Exceptions
{
    internal class TooOldExcpetion : Exception
    {
        public TooOldExcpetion(string message) : base(message) { }
    }
}
