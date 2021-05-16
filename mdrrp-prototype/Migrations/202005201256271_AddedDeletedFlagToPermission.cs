namespace mdrrp_prototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDeletedFlagToPermission : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Permissions", "DeletedFlag", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
           // DropColumn("dbo.Permissions", "DeletedFlag");
        }
    }
}
