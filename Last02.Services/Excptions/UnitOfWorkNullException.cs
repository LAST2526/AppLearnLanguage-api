using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Services.Excptions
{
    public class UnitOfWorkNullException(string message) : Exception(message)
    {
    }
}
