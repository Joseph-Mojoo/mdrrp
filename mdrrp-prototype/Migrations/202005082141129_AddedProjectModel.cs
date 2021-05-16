namespace mdrrp_prototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedProjectModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ApprovalDate = c.DateTime(nullable: false),
                        ClosingDate = c.DateTime(nullable: false),
                        Objective = c.String(nullable: false),
                        IsRegionallyTagged = c.Boolean(nullable: false),
                        BankIFCCollaboration = c.Boolean(nullable: false),
                        DeletedFlag = c.Boolean(nullable: false),
                        Budget = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Projects");
        }
    }
}
