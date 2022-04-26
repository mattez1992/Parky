using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkyAPI.DTOS
{
    public class UserReadDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Role { get; set; } = "Visitor";
        public string Token { get; set; }
    }
}
