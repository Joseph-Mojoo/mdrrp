namespace mdrrp_prototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedProjectIndicatorModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectIndicators",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Unit = c.String(nullable: false),
                        BaseLine = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Target = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DeletedFlag = c.Boolean(nullable: false),
                        ProjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId);
            
            DropTable("dbo.Indicators");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Indicators",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Type = c.String(nullable: false),
                        Unit = c.String(nullable: false),
                        BaseLine = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Target = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DataSourceMethodology = c.String(nullable: false),
                        Frequency = c.String(nullable: false),
                        DataCollector = c.String(nullable: false),
                        DeletedFlag = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.ProjectIndicators", "ProjectId", "dbo.Projects");
            DropIndex("dbo.ProjectIndicators", new[] { "ProjectId" });
            DropTable("dbo.ProjectIndicators");
        }
    }
}
