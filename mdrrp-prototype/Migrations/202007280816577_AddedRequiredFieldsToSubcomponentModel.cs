namespace mdrrp_prototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRequiredFieldsToSubcomponentModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SubComponents", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.SubComponents", "Description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SubComponents", "Description", c => c.String());
            AlterColumn("dbo.SubComponents", "Name", c => c.String());
        }
    }
}
