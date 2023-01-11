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
        T Get<T>(Expression<Func<T, bool>> filter) where T : class;
        List<T> GetSome<T>(Expression<Func<T, bool>> filter) where T : class;
    }
}
