namespace mdrrp_prototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCodePropertyForProjectModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "Code", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "Code");
        }
    }
}
