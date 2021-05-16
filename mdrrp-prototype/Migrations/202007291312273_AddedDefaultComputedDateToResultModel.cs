namespace mdrrp_prototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDefaultComputedDateToResultModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Results", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Results", "Date", c => c.DateTime(nullable: false));
        }
    }
}
