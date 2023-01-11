using CorePackage.Entities;
using CorePackage.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Examination
{
    public class StudentAnswer : IEntity
    {
        public Guid StudentAnswerId { get; set; }
        public Guid QuestionId { get; set; }
        public Question Question { get; set; }
        public Guid UserId { get; set; }
        public string StudentOpenAnswerBody { get; set; }
    }
}
