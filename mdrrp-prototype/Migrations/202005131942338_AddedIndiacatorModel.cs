namespace mdrrp_prototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedIndiacatorModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Indicators",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Type = c.String(nullable: false),
                        Unit = c.String(nullable: false),
                        BaseLine = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Target = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DataSourceMethodology = c.String(nullable: false),
                        Frequency = c.String(nullable: false),
                        DataCollector = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Indicators");
        }
    }
}
