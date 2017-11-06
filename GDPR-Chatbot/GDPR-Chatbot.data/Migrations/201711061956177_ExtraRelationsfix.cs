namespace GDPR_Chatbot.data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExtraRelationsfix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Answers", "Id", "dbo.Intents");
            DropIndex("dbo.Answers", new[] { "Id" });
            DropPrimaryKey("dbo.Answers");
            AddColumn("dbo.Intents", "Answer_Id", c => c.Int());
            AlterColumn("dbo.Answers", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Answers", "Id");
            CreateIndex("dbo.Intents", "Answer_Id");
            AddForeignKey("dbo.Intents", "Answer_Id", "dbo.Answers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Intents", "Answer_Id", "dbo.Answers");
            DropIndex("dbo.Intents", new[] { "Answer_Id" });
            DropPrimaryKey("dbo.Answers");
            AlterColumn("dbo.Answers", "Id", c => c.Int(nullable: false));
            DropColumn("dbo.Intents", "Answer_Id");
            AddPrimaryKey("dbo.Answers", "Id");
            CreateIndex("dbo.Answers", "Id");
            AddForeignKey("dbo.Answers", "Id", "dbo.Intents", "Id");
        }
    }
}
