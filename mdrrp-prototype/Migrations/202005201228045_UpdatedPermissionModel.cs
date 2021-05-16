namespace mdrrp_prototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedPermissionModel : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Permissions", "Description", c => c.String(nullable: false));
            //DropColumn("dbo.Permissions", "Module");
            //DropColumn("dbo.Permissions", "Desscription");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.Permissions", "Desscription", c => c.String(nullable: false));
            //AddColumn("dbo.Permissions", "Module", c => c.String(nullable: false));
            //DropColumn("dbo.Permissions", "Description");
        }
    }
}
