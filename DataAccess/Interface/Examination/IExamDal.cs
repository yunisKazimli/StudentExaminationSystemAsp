using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interface.Examination
{
    public interface IExamDal
    {
        void Add<T>(T entity);
        void Delete<T>(T entity) where T : class;
        T Get<T>(Expression<Func<T, bool>> filter) where T : class;
        List<T> GetSome<T>(Expression<Func<T, bool>> filter) where T : class;
    }
}
