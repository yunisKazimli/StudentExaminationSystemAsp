using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Examination.GetDTOs
{
    public class StudentAnswerGetDTO
    {
        public Guid StudentAnswerId { get; set; }
        public Guid GroupId { get; set; }
        public Guid UserId { get; set; }
        public Guid QuestionId { get; set; }
        public List<OptionGetDTO> Options { get; set; }
        public string OpenAnswer { get; set; }
    }
}
