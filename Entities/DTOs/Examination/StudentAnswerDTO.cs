using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Examination
{
    public class StudentAnswerDTO
    {
        public string QuestionId { get; set; }
        public string AnswerBody { get; set; }
        public List<string> OptionsId { get; set; }
    }
}
