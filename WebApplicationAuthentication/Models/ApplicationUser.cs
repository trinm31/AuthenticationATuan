using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace WebApplicationAuthentication.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        [NotMapped] 
        public string Role { get; set; }
    }
}