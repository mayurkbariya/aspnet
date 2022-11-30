using Microsoft.AspNetCore.Identity;

namespace FBDropshipper.Domain.Entities
{
    public class RoleClaim : IdentityRoleClaim<string>
    {
        public Role Role { get; set; }        
    }
}