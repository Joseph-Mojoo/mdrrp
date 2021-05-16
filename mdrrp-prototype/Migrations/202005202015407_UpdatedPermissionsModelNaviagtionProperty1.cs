namespace mdrrp_prototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedPermissionsModelNaviagtionProperty1 : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.Permissions", "ApplicationRole_Id", "dbo.AspNetRoles");
            //DropForeignKey("dbo.AspNetRoles", "Permission_Id", "dbo.Permissions");
            //DropIndex("dbo.Permissions", new[] { "ApplicationRole_Id" });
            //DropIndex("dbo.AspNetRoles", new[] { "Permission_Id" });
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
            
            //DropColumn("dbo.Permissions", "ApplicationRole_Id");
            //DropColumn("dbo.AspNetRoles", "Permission_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetRoles", "Permission_Id", c => c.Int());
            AddColumn("dbo.Permissions", "ApplicationRole_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.ApplicationRolePermissions", "Permission_Id", "dbo.Permissions");
            DropForeignKey("dbo.ApplicationRolePermissions", "ApplicationRole_Id", "dbo.AspNetRoles");
            DropIndex("dbo.ApplicationRolePermissions", new[] { "Permission_Id" });
            DropIndex("dbo.ApplicationRolePermissions", new[] { "ApplicationRole_Id" });
            DropTable("dbo.ApplicationRolePermissions");
            CreateIndex("dbo.AspNetRoles", "Permission_Id");
            CreateIndex("dbo.Permissions", "ApplicationRole_Id");
            AddForeignKey("dbo.AspNetRoles", "Permission_Id", "dbo.Permissions", "Id");
            AddForeignKey("dbo.Permissions", "ApplicationRole_Id", "dbo.AspNetRoles", "Id");
        }
    }
}
