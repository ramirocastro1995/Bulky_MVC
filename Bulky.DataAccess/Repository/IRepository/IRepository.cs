using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository.IRepository
{
    
    public interface IRepository<T> where T : class
    {
        //T in this case will be Category
        IEnumerable<T> GetAll();
        T Get(Expression<Func<T,bool>>filter);
        void Add(T entity);
        //Leave Update to specific repository of the specific class
        //void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}
