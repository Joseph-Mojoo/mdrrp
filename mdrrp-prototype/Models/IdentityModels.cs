using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace mdrrp_prototype.Models
{
   
// You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
public class ApplicationUser : IdentityUser
{
    public ApplicationUser()
    {
        this.Projects = new HashSet<Project>();
        this.SubComponents = new HashSet<SubComponent>();
    }
    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, string> manager)
    {
        // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
        var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
        // Add custom user claims here
        userIdentity.AddClaim(new Claim("UserName", FirstName));
        return userIdentity;
    }
    public string Title { get; set; }


    public string FirstName { get; set; }


    public string LastName { get; set; }

    public ICollection<Project> Projects { get; set; }

    public ICollection<SubComponent> SubComponents { get; set; }
}

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<District> Districts { get; set; }
    public DbSet<ProjectComponent> ProjectComponents { get; set; }
    public DbSet<SubComponent> SubComponents { get; set; }
    public DbSet<ProjectIndicator> ProjectIndicators { get; set; }
    public DbSet<IntermediateIndicator> IntermediateIndicators { get; set; }
    public DbSet<Result> Results { get; set; }
    public DbSet<Permission> Permissions { get; set; }


    public ApplicationDbContext()
        : base("DefaultConnection")
    {
    }

    public static ApplicationDbContext Create()
    {
        return new ApplicationDbContext();
    }

       
    }

}