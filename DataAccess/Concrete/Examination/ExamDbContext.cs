using CorePackage.Entities.Concrete;
using Entities.Concrete.Examination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.Examination
{
    public class ExamDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server = DESKTOP-IAADCGV\SQLSERVER; Database = StudentExaminationSystem; Trusted_Connection = True; TrustServerCertificate = True; ");
        }

        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<StudentAnswerOption> StudentAnswerOptions { get; set; }
        public DbSet<StudentAnswer> StudentAnswers { get; set; }
    }
}
