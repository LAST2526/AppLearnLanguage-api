using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Services.Interfaces
{
    public interface IBaseService : IDisposable
    {
        IQueryable<T>? OrderByDynamic<T>(IQueryable<T> source, string propertyName, bool ascending);
    }
}
