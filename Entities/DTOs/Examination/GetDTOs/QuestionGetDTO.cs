using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Examination.GetDTOs
{
    public class QuestionGetDTO
    {
        public Guid QuestionId { get; set; }
        public Guid GroupId { get; set; }
        public string QuestionBody { get; set; }
        public List<OptionGetDTO> Options { get; set; }
    }
}
