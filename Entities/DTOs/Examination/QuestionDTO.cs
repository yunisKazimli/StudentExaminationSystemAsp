using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Examination
{
    public class QuestionDTO
    {
        public string QuestionBody { get; set; }
        public string GroupId { get; set; }
        public List<OptionDTO> Options { get; set; }
    }
}
