namespace mdrrp_prototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddePermissionModel : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //        "dbo.Permissions",
            //        c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Description = c.String(false),
            //            DeletedFlag = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
            DropTable("dbo.Permissions");
        }
    }
}
