using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Data.Repositories.RepositoryBase
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}
