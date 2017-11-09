namespace GDPR_Chatbot.data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNeededEntitiesToAnswer1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Entities", "Answer_Id", c => c.Int());
            CreateIndex("dbo.Entities", "Answer_Id");
            AddForeignKey("dbo.Entities", "Answer_Id", "dbo.Answers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Entities", "Answer_Id", "dbo.Answers");
            DropIndex("dbo.Entities", new[] { "Answer_Id" });
            DropColumn("dbo.Entities", "Answer_Id");
        }
    }
}
