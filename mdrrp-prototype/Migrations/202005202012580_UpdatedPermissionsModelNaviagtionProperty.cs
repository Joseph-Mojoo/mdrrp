namespace mdrrp_prototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedPermissionsModelNaviagtionProperty : DbMigration
    {
        public override void Up()
        {
            //DropIndex("dbo.Roles", new[] { "Permission_Id" });
            //AddColumn("dbo.AspNetRoles", "Permission_Id", c => c.Int());
            //CreateIndex("dbo.AspNetRoles", "Permission_Id");
            //DropTable("dbo.Roles");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                        Permission_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropIndex("dbo.AspNetRoles", new[] { "Permission_Id" });
            DropColumn("dbo.AspNetRoles", "Permission_Id");
            CreateIndex("dbo.Roles", "Permission_Id");
        }
    }
}
