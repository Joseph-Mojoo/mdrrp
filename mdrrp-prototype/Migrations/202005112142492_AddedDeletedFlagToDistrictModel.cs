namespace mdrrp_prototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDeletedFlagToDistrictModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Districts", "DeletedFlag", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Districts", "DeletedFlag");
        }
    }
}
