namespace GDPR_Chatbot.data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExtraRelations : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Answers");
            AddColumn("dbo.Answers", "Entity_Id", c => c.Int());
            AlterColumn("dbo.Answers", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Answers", "Id");
            CreateIndex("dbo.Answers", "Id");
            CreateIndex("dbo.Answers", "Entity_Id");
            AddForeignKey("dbo.Answers", "Entity_Id", "dbo.Entities", "Id");
            AddForeignKey("dbo.Answers", "Id", "dbo.Intents", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Answers", "Id", "dbo.Intents");
            DropForeignKey("dbo.Answers", "Entity_Id", "dbo.Entities");
            DropIndex("dbo.Answers", new[] { "Entity_Id" });
            DropIndex("dbo.Answers", new[] { "Id" });
            DropPrimaryKey("dbo.Answers");
            AlterColumn("dbo.Answers", "Id", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Answers", "Entity_Id");
            AddPrimaryKey("dbo.Answers", "Id");
        }
    }
}
