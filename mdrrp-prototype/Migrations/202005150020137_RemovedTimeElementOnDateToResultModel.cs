namespace mdrrp_prototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedTimeElementOnDateToResultModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Results", "Date", c => c.DateTime(nullable: false));
            DropColumn("dbo.Results", "DateTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Results", "DateTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Results", "Date");
        }
    }
}
