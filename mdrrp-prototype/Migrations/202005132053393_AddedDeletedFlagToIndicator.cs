namespace mdrrp_prototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDeletedFlagToIndicator : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Indicators", "DeletedFlag", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Indicators", "DeletedFlag");
        }
    }
}
