using CorePackage.Entities.Concrete;
using Entities.DTOs.Identity.GetDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Examination.GetDTOs
{
    public class GroupGetDTO
    {
        public Guid GroupId { get; set; }
        public string GroupName { get; set; }
        public UserGetDTO Instructor { get; set; }
        public List<UserGetDTO> Students { get; set; }
    }
}
