using Microsoft.AspNetCore.Identity;

namespace WebApp.Template.Models
{
    public class AppUser : IdentityUser
    {
        public string ImageUrl { get; set; }
        public string Description{ get; set; }
    }
}