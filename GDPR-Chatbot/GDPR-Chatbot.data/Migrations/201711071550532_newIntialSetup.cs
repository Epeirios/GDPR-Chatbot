namespace GDPR_Chatbot.data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newIntialSetup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FromId = c.String(),
                        FromName = c.String(),
                        RecipientId = c.String(),
                        RecipientName = c.String(),
                        TextFormat = c.String(),
                        TopicName = c.String(),
                        HistoryDisclosed = c.Boolean(nullable: false),
                        Local = c.String(),
                        Text = c.String(),
                        Summary = c.String(),
                        ChannelId = c.String(),
                        ServiceUrl = c.String(),
                        ReplyToId = c.String(),
                        Action = c.String(),
                        Type = c.String(),
                        Timestamp = c.DateTimeOffset(nullable: false, precision: 7),
                        ConversationId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AnswerText = c.String(),
                        Type = c.Int(nullable: false),
                        TotalReviews = c.Int(nullable: false),
                        PositiveReviews = c.Int(nullable: false),
                        ActiveQuestion = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Entities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Question = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Intents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Answer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Answers", t => t.Answer_Id)
                .Index(t => t.Answer_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Intents", "Answer_Id", "dbo.Answers");
            DropIndex("dbo.Intents", new[] { "Answer_Id" });
            DropTable("dbo.Intents");
            DropTable("dbo.Entities");
            DropTable("dbo.Answers");
            DropTable("dbo.Activities");
        }
    }
}
