using CorePackage.DataAccess;
using CorePackage.Entities;
using CorePackage.Entities.Concrete;
using DataAccess.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interface.Identity
{
    public interface IAuthDal
    {
        void Add<T>(T entity);
        T Get<T>(Expression<Func<T, bool>> filter) where T : class;
        List<T> GetSome<T>(Expression<Func<T, bool>> filter) where T : class;
    }
}
