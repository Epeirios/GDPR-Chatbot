namespace GDPR_Chatbot.data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAnswersToEntities : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Entities", "Answer_Id", "dbo.Answers");
            DropIndex("dbo.Entities", new[] { "Answer_Id" });
            CreateTable(
                "dbo.EntityAnswers",
                c => new
                    {
                        Entity_Id = c.Int(nullable: false),
                        Answer_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Entity_Id, t.Answer_Id })
                .ForeignKey("dbo.Entities", t => t.Entity_Id, cascadeDelete: true)
                .ForeignKey("dbo.Answers", t => t.Answer_Id, cascadeDelete: true)
                .Index(t => t.Entity_Id)
                .Index(t => t.Answer_Id);
            
            DropColumn("dbo.Entities", "Answer_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Entities", "Answer_Id", c => c.Int());
            DropForeignKey("dbo.EntityAnswers", "Answer_Id", "dbo.Answers");
            DropForeignKey("dbo.EntityAnswers", "Entity_Id", "dbo.Entities");
            DropIndex("dbo.EntityAnswers", new[] { "Answer_Id" });
            DropIndex("dbo.EntityAnswers", new[] { "Entity_Id" });
            DropTable("dbo.EntityAnswers");
            CreateIndex("dbo.Entities", "Answer_Id");
            AddForeignKey("dbo.Entities", "Answer_Id", "dbo.Answers", "Id");
        }
    }
}
