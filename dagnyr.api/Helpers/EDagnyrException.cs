using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dagnyr.api.Helpers;

public class EDagnyrException : Exception
{
    public EDagnyrException(string message) : base(message)
    {
    }
}
