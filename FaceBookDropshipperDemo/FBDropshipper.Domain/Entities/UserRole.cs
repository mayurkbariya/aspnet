using Microsoft.AspNetCore.Identity;

namespace FBDropshipper.Domain.Entities
{
    public class UserRole : IdentityUserRole<string>
    {
        public Role Role { get; set; }
        public User User { get; set; }
    }
}