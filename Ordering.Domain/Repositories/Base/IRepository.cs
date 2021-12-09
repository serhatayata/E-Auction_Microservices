using Ordering.Domain.Entities;
using Ordering.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Repositories.Base
{
    public interface IRepository<T> where T:Entity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate=null,
                                      Func<IQueryable<T>,IOrderedQueryable<T>> orderby=null, //burada Orderby yapılmak istenen
                                      string includeString = null, //Bu kısımda lazy loading için kullanılır
                                      bool disableTracking=true //Entity framework performance optimizer yaptığımız parametresi.
                                     );
        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T Entity);
        Task UpdateAsync(T Entity);
        Task DeleteAsync(T Entity);

    }
}
