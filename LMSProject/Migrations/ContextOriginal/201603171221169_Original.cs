namespace LMSProject.Migrations.ContextOriginal
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Original : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.files",
                c => new
                    {
                        fileID = c.Int(nullable: false, identity: true),
                        folderID = c.Int(nullable: false),
                        name = c.String(nullable: false),
                        path = c.String(nullable: false),
                        taskID = c.Int(),
                    })
                .PrimaryKey(t => t.fileID)
                .ForeignKey("dbo.folders", t => t.folderID)
                .ForeignKey("dbo.tasks", t => t.taskID)
                .Index(t => t.folderID)
                .Index(t => t.taskID);
            
            CreateTable(
                "dbo.folders",
                c => new
                    {
                        folderID = c.Int(nullable: false, identity: true),
                        folderTypeID = c.Int(nullable: false),
                        schoolClassID = c.Int(nullable: false),
                        name = c.String(nullable: false),
                        path = c.String(),
                    })
                .PrimaryKey(t => t.folderID)
                .ForeignKey("dbo.schoolClasses", t => t.schoolClassID)
                .Index(t => t.schoolClassID);
            
            CreateTable(
                "dbo.schoolClasses",
                c => new
                    {
                        schoolClassID = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.schoolClassID);
            
            CreateTable(
                "dbo.tasks",
                c => new
                    {
                        taskID = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false),
                        userID = c.Int(nullable: false),
                        schoolClassID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.taskID)
                .ForeignKey("dbo.schoolClasses", t => t.schoolClassID)
                .ForeignKey("dbo.user_teacher", t => t.userID)
                .Index(t => t.userID)
                .Index(t => t.schoolClassID);
            
            CreateTable(
                "dbo.user_teacher",
                c => new
                    {
                        user_teacherID = c.Int(nullable: false, identity: true),
                        userId = c.String(nullable: false),
                        schoolClassID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.user_teacherID)
                .ForeignKey("dbo.schoolClasses", t => t.schoolClassID)
                .Index(t => t.schoolClassID);
            
            CreateTable(
                "dbo.folderTypes",
                c => new
                    {
                        folderTypeID = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.folderTypeID);
            
            CreateTable(
                "dbo.scheduleDetails",
                c => new
                    {
                        scheduleDetailID = c.Int(nullable: false, identity: true),
                        scheduleID = c.Int(nullable: false),
                        startTime = c.DateTime(nullable: false),
                        endTime = c.DateTime(nullable: false),
                        name = c.String(nullable: false),
                        room = c.String(),
                        taskID = c.Int(),
                    })
                .PrimaryKey(t => t.scheduleDetailID)
                .ForeignKey("dbo.schedules", t => t.scheduleID)
                .ForeignKey("dbo.tasks", t => t.taskID)
                .Index(t => t.scheduleID)
                .Index(t => t.taskID);
            
            CreateTable(
                "dbo.schedules",
                c => new
                    {
                        scheduleID = c.Int(nullable: false, identity: true),
                        schoolClassID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.scheduleID)
                .ForeignKey("dbo.schoolClasses", t => t.schoolClassID)
                .Index(t => t.schoolClassID);
            
            CreateTable(
                "dbo.user_student",
                c => new
                    {
                        userId = c.String(nullable: false, maxLength: 128),
                        schoolClassID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.userId)
                .ForeignKey("dbo.schoolClasses", t => t.schoolClassID)
                .Index(t => t.schoolClassID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.user_student", "schoolClassID", "dbo.schoolClasses");
            DropForeignKey("dbo.scheduleDetails", "taskID", "dbo.tasks");
            DropForeignKey("dbo.scheduleDetails", "scheduleID", "dbo.schedules");
            DropForeignKey("dbo.schedules", "schoolClassID", "dbo.schoolClasses");
            DropForeignKey("dbo.files", "taskID", "dbo.tasks");
            DropForeignKey("dbo.tasks", "userID", "dbo.user_teacher");
            DropForeignKey("dbo.user_teacher", "schoolClassID", "dbo.schoolClasses");
            DropForeignKey("dbo.tasks", "schoolClassID", "dbo.schoolClasses");
            DropForeignKey("dbo.files", "folderID", "dbo.folders");
            DropForeignKey("dbo.folders", "schoolClassID", "dbo.schoolClasses");
            DropIndex("dbo.user_student", new[] { "schoolClassID" });
            DropIndex("dbo.schedules", new[] { "schoolClassID" });
            DropIndex("dbo.scheduleDetails", new[] { "taskID" });
            DropIndex("dbo.scheduleDetails", new[] { "scheduleID" });
            DropIndex("dbo.user_teacher", new[] { "schoolClassID" });
            DropIndex("dbo.tasks", new[] { "schoolClassID" });
            DropIndex("dbo.tasks", new[] { "userID" });
            DropIndex("dbo.folders", new[] { "schoolClassID" });
            DropIndex("dbo.files", new[] { "taskID" });
            DropIndex("dbo.files", new[] { "folderID" });
            DropTable("dbo.user_student");
            DropTable("dbo.schedules");
            DropTable("dbo.scheduleDetails");
            DropTable("dbo.folderTypes");
            DropTable("dbo.user_teacher");
            DropTable("dbo.tasks");
            DropTable("dbo.schoolClasses");
            DropTable("dbo.folders");
            DropTable("dbo.files");
        }
    }
}
