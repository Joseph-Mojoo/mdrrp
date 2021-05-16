namespace mdrrp_prototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPivotTableLinkingUserToSubcomponent : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationUserSubComponents",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        SubComponent_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.SubComponent_Id })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.SubComponents", t => t.SubComponent_Id, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.SubComponent_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationUserSubComponents", "SubComponent_Id", "dbo.SubComponents");
            DropForeignKey("dbo.ApplicationUserSubComponents", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ApplicationUserSubComponents", new[] { "SubComponent_Id" });
            DropIndex("dbo.ApplicationUserSubComponents", new[] { "ApplicationUser_Id" });
            DropTable("dbo.ApplicationUserSubComponents");
        }
    }
}
