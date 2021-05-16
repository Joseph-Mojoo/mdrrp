namespace mdrrp_prototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedProjectIndicatorModel1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.IntermediateIndicators",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Unit = c.String(nullable: false),
                        BaseLine = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Target = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DataSourceMethodology = c.String(nullable: false),
                        Frequency = c.String(nullable: false),
                        DataCollector = c.String(nullable: false),
                        DeletedFlag = c.Boolean(nullable: false),
                        ProjectIndicatorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProjectIndicators", t => t.ProjectIndicatorId, cascadeDelete: true)
                .Index(t => t.ProjectIndicatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IntermediateIndicators", "ProjectIndicatorId", "dbo.ProjectIndicators");
            DropIndex("dbo.IntermediateIndicators", new[] { "ProjectIndicatorId" });
            DropTable("dbo.IntermediateIndicators");
        }
    }
}
