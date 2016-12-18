namespace Bookvoed.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        nameAuthor = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.nameAuthor);
            
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        BookId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Series = c.String(),
                        Subject = c.String(),
                        Year = c.String(),
                        Author_nameAuthor = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.BookId)
                .ForeignKey("dbo.Authors", t => t.Author_nameAuthor)
                .Index(t => t.Author_nameAuthor);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Books", "Author_nameAuthor", "dbo.Authors");
            DropIndex("dbo.Books", new[] { "Author_nameAuthor" });
            DropTable("dbo.Books");
            DropTable("dbo.Authors");
        }
    }
}
