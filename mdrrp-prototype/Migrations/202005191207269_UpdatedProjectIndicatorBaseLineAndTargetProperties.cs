namespace mdrrp_prototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedProjectIndicatorBaseLineAndTargetProperties : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProjectIndicators", "BaseLine", c => c.Int(nullable: false));
            AlterColumn("dbo.ProjectIndicators", "Target", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProjectIndicators", "Target", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ProjectIndicators", "BaseLine", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
