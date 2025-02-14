using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTOs.SystemAccountDTOs
{
    public class LoginDTO
    {
        public string AccountEmail { get; set; } = string.Empty;
        public string AccountPassword { get; set; } = string.Empty;
    }
}
