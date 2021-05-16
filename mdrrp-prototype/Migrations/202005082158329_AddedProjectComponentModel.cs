namespace mdrrp_prototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedProjectComponentModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectComponents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Budget = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Description = c.String(nullable: false),
                        ProjectId = c.Int(nullable: false),
                        DeletedFlag = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectComponents", "ProjectId", "dbo.Projects");
            DropIndex("dbo.ProjectComponents", new[] { "ProjectId" });
            DropTable("dbo.ProjectComponents");
        }
    }
}
