using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NB.Core.Web.DataAccess.Repository
{
    interface IEventSourceRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        IEnumerable<T> GetByTicker(string ticker);
        bool Add(T entity);
    }
}
