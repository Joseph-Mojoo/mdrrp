namespace mdrrp_prototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPermissionsNavigationToIdentityRole : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Permissions", "ApplicationRole_Id", c => c.String(maxLength: 128));
            //AddColumn("dbo.AspNetRoles", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            //("dbo.Permissions", "ApplicationRole_Id");
           // AddForeignKey("dbo.Permissions", "ApplicationRole_Id", "dbo.AspNetRoles", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Permissions", "ApplicationRole_Id", "dbo.AspNetRoles");
            DropIndex("dbo.Permissions", new[] { "ApplicationRole_Id" });
            DropColumn("dbo.AspNetRoles", "Discriminator");
            DropColumn("dbo.Permissions", "ApplicationRole_Id");
        }
    }
}
