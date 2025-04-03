using IdentityAPI.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityAPI.Data;

public class ApplicationIdentityDbContext 
    : IdentityDbContext<ApplicationIdentityUser, ApplicationIdentityRole, Guid>
{
    public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options) 
        : base(options) { }
}
