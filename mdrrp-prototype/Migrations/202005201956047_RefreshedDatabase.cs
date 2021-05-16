namespace mdrrp_prototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RefreshedDatabase : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.Permissions", "ApplicationRole_Id", "dbo.AspNetRoles");
            //DropIndex("dbo.Permissions", new[] { "ApplicationRole_Id" });
           // DropColumn("dbo.Permissions", "ApplicationRole_Id");
            //DropColumn("dbo.AspNetRoles", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetRoles", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Permissions", "ApplicationRole_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Permissions", "ApplicationRole_Id");
            AddForeignKey("dbo.Permissions", "ApplicationRole_Id", "dbo.AspNetRoles", "Id");
        }
    }
}
