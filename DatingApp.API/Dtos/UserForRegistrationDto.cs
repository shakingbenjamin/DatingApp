using System;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    public class UserForRegistrationDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "Must be correctly formatted email address.")]
        public string Username { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 20 characters.")]
        public string Password { get; set; }
        
        [Required]
        public string Gender { get; set; }
       
        [Required]
        public string KnownAs { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }
        public DateTime LastActive { get; set; }
        public DateTime Created { get; set; }
        public UserForRegistrationDto()
        {
            this.LastActive = DateTime.Now;
            this.Created = DateTime.Now;   
        }
    }
}