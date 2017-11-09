namespace GDPR_Chatbot.data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixedRelation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Intents", "Answer_Id", "dbo.Answers");
            DropIndex("dbo.Intents", new[] { "Answer_Id" });
            AddColumn("dbo.Answers", "Intent_Id", c => c.Int());
            CreateIndex("dbo.Answers", "Intent_Id");
            AddForeignKey("dbo.Answers", "Intent_Id", "dbo.Intents", "Id");
            DropColumn("dbo.Intents", "Answer_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Intents", "Answer_Id", c => c.Int());
            DropForeignKey("dbo.Answers", "Intent_Id", "dbo.Intents");
            DropIndex("dbo.Answers", new[] { "Intent_Id" });
            DropColumn("dbo.Answers", "Intent_Id");
            CreateIndex("dbo.Intents", "Answer_Id");
            AddForeignKey("dbo.Intents", "Answer_Id", "dbo.Answers", "Id");
        }
    }
}
