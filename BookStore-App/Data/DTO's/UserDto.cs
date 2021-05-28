using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_App.Data.DTO_s
{
    public class UserDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required]
        [StringLength(15, ErrorMessage = "Your Password is limited to {2} to {1} characters",MinimumLength =6)]
        public string Password { get; set; }
    }
}
