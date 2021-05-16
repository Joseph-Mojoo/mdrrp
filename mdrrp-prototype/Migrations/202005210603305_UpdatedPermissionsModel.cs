namespace mdrrp_prototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedPermissionsModel : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.RolePermissions", "Role_Id", "dbo.Roles");
            //DropForeignKey("dbo.RolePermissions", "Permission_Id", "dbo.Permissions");
            //DropForeignKey("dbo.Permissions", "ApplicationRole_Id", "dbo.AspNetRoles");
            ////DropIndex("dbo.Permissions", new[] { "ApplicationRole_Id" });
            //DropIndex("dbo.RolePermissions", new[] { "Role_Id" });
            //DropIndex("dbo.RolePermissions", new[] { "Permission_Id" });
            //CreateTable(
            //    "dbo.ApplicationRolePermissions",
            //    c => new
            //        {
            //            ApplicationRole_Id = c.String(nullable: false, maxLength: 128),
            //            Permission_Id = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => new { t.ApplicationRole_Id, t.Permission_Id })
            //    .ForeignKey("dbo.AspNetRoles", t => t.ApplicationRole_Id, cascadeDelete: true)
            //    .ForeignKey("dbo.Permissions", t => t.Permission_Id, cascadeDelete: true)
            //    .Index(t => t.ApplicationRole_Id)
            //    .Index(t => t.Permission_Id);
            
            ////DropColumn("dbo.Permissions", "ApplicationRole_Id");
            //DropTable("dbo.Roles");
            //DropTable("dbo.RolePermissions");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.RolePermissions",
                c => new
                    {
                        Role_Id = c.String(nullable: false, maxLength: 128),
                        Permission_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Role_Id, t.Permission_Id });
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Permissions", "ApplicationRole_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.ApplicationRolePermissions", "Permission_Id", "dbo.Permissions");
            DropForeignKey("dbo.ApplicationRolePermissions", "ApplicationRole_Id", "dbo.AspNetRoles");
            DropIndex("dbo.ApplicationRolePermissions", new[] { "Permission_Id" });
            DropIndex("dbo.ApplicationRolePermissions", new[] { "ApplicationRole_Id" });
            DropTable("dbo.ApplicationRolePermissions");
            CreateIndex("dbo.RolePermissions", "Permission_Id");
            CreateIndex("dbo.RolePermissions", "Role_Id");
            CreateIndex("dbo.Permissions", "ApplicationRole_Id");
            AddForeignKey("dbo.Permissions", "ApplicationRole_Id", "dbo.AspNetRoles", "Id");
            AddForeignKey("dbo.RolePermissions", "Permission_Id", "dbo.Permissions", "Id", cascadeDelete: true);
            AddForeignKey("dbo.RolePermissions", "Role_Id", "dbo.Roles", "Id", cascadeDelete: true);
        }
    }
}
