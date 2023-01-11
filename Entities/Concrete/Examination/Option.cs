using CorePackage.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Examination
{
    public class Option : IEntity
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
        public Guid QuestionId { get; set; }
        public Question Question { get; set; }
        public bool IsRight { get; set; }
    }
}
