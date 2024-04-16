using Microsoft.AspNetCore.Identity;

namespace MasterGameMaster.Domain.User
{
    public class AppUser : IdentityUser<Guid>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}
