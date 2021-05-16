namespace mdrrp_prototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateIntermediateIndicatorBaseLineAndTargetDataType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.IntermediateIndicators", "BaseLine", c => c.Int(nullable: false));
            AlterColumn("dbo.IntermediateIndicators", "Target", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.IntermediateIndicators", "Target", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.IntermediateIndicators", "BaseLine", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
