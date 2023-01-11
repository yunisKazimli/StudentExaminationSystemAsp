using CorePackage.Entities;
using CorePackage.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Examination
{
    public class UserGroup : IEntity
    {
        public Guid UserGroupId { get; set; }
        public Guid UserId { get; set; }
        public Guid GroupId { get; set; }
        public Group Group { get; set; }
    }
}
