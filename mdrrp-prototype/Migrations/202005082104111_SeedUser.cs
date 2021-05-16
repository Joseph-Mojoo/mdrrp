namespace mdrrp_prototype.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedUser : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                    INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'85d42e53-c0eb-4c2e-8f7c-5d10d4f0ad33', N'administrator')
                    INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'0278448f-ab03-46d2-b6d2-90a4aede4ca2', N'admin@mdrrp.com', 0, N'AATKZIXgF2KXaex+xQIZE0oK6JNkWdP+T5L4yWb52yyROJVihJ1l9NR0O4JRRvdhlw==', N'45753dd3-05e9-4aa9-9c4f-a0e8cae7a999', NULL, 0, 0, NULL, 1, 0, N'admin@mdrrp.com')
                    INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'0278448f-ab03-46d2-b6d2-90a4aede4ca2', N'85d42e53-c0eb-4c2e-8f7c-5d10d4f0ad33')
                   
                   

                ");
        }
        
        public override void Down()
        {
        }
    }
}
