namespace mdrrp_prototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSubComponentModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SubComponents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Budget = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Description = c.String(),
                        DeletedFlag = c.Boolean(nullable: false),
                        ProjectComponentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProjectComponents", t => t.ProjectComponentId, cascadeDelete: true)
                .Index(t => t.ProjectComponentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubComponents", "ProjectComponentId", "dbo.ProjectComponents");
            DropIndex("dbo.SubComponents", new[] { "ProjectComponentId" });
            DropTable("dbo.SubComponents");
        }
    }
}
