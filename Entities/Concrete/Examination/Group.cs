using CorePackage.Entities;
using CorePackage.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Examination
{
    public class Group : IEntity
    {
        public Guid GroupId { get; set; }
        public string GroupName { get; set; }
        public bool IsActive { get; set; }
    }
}
