using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.DTOs
{
    public class UserRegisterDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "Hasło musi zawierać co najmniej 4 i maksimum 8 znaków.")]
        public string Password { get; set; }
    }
}
