using Entities.Concrete.Examination;
using Entities.DTOs.Examination.GetDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Identity.GetDTOs
{
    public class UserGetDTO
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public List<GroupGetDTO> Groups { get; set; }
    }
}
