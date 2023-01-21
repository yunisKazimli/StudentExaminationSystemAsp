using CorePackage.DataAccess.EntityFramework;
using CorePackage.Entities;
using CorePackage.Entities.Concrete;
using DataAccess.Interface.Identity;
using Microsoft.EntityFrameworkCore;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.Identity
{
    public class AuthDal : AuthDbContext, IAuthDal
    {
        public T Get<T>(Expression<Func<T, bool>> filter) where T : class
        {
            using var context = new AuthDbContext();
            return context.Set<T>().SingleOrDefault(filter);
        }

        public void Add<T>(T entity)
        {
            using var context = new AuthDbContext();
            var addEntity = context.Entry(entity);
            addEntity.State = EntityState.Added;
            context.SaveChanges();
        }

        public List<T> GetSome<T>(Expression<Func<T, bool>> filter) where T : class
        {
            using var context = new AuthDbContext();
            return context.Set<T>().Where(filter).ToList();
        }

        public void Delete<T>(T entity) where T : class
        {
            using var context = new AuthDbContext();
            var addEntity = context.Remove<T>(entity);
            addEntity.State = EntityState.Deleted;
            context.SaveChanges();
        }
    }
}
