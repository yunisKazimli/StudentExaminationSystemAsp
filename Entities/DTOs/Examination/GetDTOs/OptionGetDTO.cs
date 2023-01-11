using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Examination.GetDTOs
{
    public class OptionGetDTO
    {
        public Guid OptionId { get; set; }
        public string OptionBody { get; set; }
        public bool IsRight { get; set; }
    }
}
