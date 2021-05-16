namespace mdrrp_prototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedResultModule : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Results",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IntermediateIndicatorId = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DeletedFlag = c.Boolean(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IntermediateIndicators", t => t.IntermediateIndicatorId, cascadeDelete: true)
                .Index(t => t.IntermediateIndicatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Results", "IntermediateIndicatorId", "dbo.IntermediateIndicators");
            DropIndex("dbo.Results", new[] { "IntermediateIndicatorId" });
            DropTable("dbo.Results");
        }
    }
}
