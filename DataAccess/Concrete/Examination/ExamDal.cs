using DataAccess.Concrete.Identity;
using DataAccess.Interface.Examination;
using DataAccess.Interface.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.Examination
{
    public class ExamDal : IExamDal
    {
        public T Get<T>(Expression<Func<T, bool>> filter) where T : class
        {
            using var context = new ExamDbContext();
            return context.Set<T>().SingleOrDefault(filter);
        }

        public void Add<T>(T entity)
        {
            using var context = new ExamDbContext();
            var addEntity = context.Entry(entity);
            addEntity.State = EntityState.Added;
            context.SaveChanges();
        }

        public List<T> GetSome<T>(Expression<Func<T, bool>> filter) where T : class
        {
            using var context = new ExamDbContext();
            return context.Set<T>().ToList();
        }

        public void Delete<T>(T entity) where T : class
        {
            using var context = new ExamDbContext();
            var deleteEntity = context.Remove(entity);
            deleteEntity.State = EntityState.Deleted;
            context.SaveChanges();
        }
    }
}
