using Microsoft.AspNetCore.Identity;

namespace MasterGameMaster.Domain.User
{
    public class AppRole : IdentityRole<Guid>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}
