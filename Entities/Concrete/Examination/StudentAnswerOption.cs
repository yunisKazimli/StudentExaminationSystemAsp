using CorePackage.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Examination
{
    public class StudentAnswerOption : IEntity
    {
        public Guid StudentAnswerOptionId { get; set; }
        public Guid OptionId { get; set; }
        public Option Option { get; set; }
        public Guid StudentAnswerId { get; set; }
        public StudentAnswer StudentAnswer { get; set; }
    }
}
