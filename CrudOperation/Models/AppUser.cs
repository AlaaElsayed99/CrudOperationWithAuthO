using Microsoft.AspNetCore.Identity;

namespace CrudOperation.Models
{
    public class AppUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public byte[]? Image { get; set; }


    }
}
