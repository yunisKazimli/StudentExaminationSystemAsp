using CorePackage.Entities;
using CorePackage.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Examination
{
    public class Question : IEntity
    {
        public Guid QuestionId { get; set; }
        public string QuestionBody { get; set; }
        public Guid GroupId { get; set; }
        public Group Group { get; set; }
    }
}
