namespace mdrrp_prototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedIntermediateIndicatorModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IntermediateIndicators", "SubComponentId", c => c.Int());
            CreateIndex("dbo.IntermediateIndicators", "SubComponentId");
            AddForeignKey("dbo.IntermediateIndicators", "SubComponentId", "dbo.SubComponents", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IntermediateIndicators", "SubComponentId", "dbo.SubComponents");
            DropIndex("dbo.IntermediateIndicators", new[] { "SubComponentId" });
            DropColumn("dbo.IntermediateIndicators", "SubComponentId");
        }
    }
}
