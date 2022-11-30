using Microsoft.AspNetCore.Identity;

namespace FBDropshipper.Domain.Entities
{
    public class UserClaim : IdentityUserClaim<string>
    {
        public User User { get; set; }    
    }
}