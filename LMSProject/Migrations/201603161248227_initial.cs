namespace LMSProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.files",
                c => new
                    {
                        fileID = c.Int(nullable: false, identity: true),
                        folderID = c.Int(nullable: false),
                        name = c.String(),
                        path = c.String(),
                        taskID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.fileID)
                .ForeignKey("dbo.folders", t => t.folderID, cascadeDelete: true)
                .ForeignKey("dbo.tasks", t => t.taskID, cascadeDelete: true)
                .Index(t => t.folderID)
                .Index(t => t.taskID);
            
            CreateTable(
                "dbo.folders",
                c => new
                    {
                        folderID = c.Int(nullable: false, identity: true),
                        folderTypeID = c.Int(nullable: false),
                        schoolClassID = c.Int(nullable: false),
                        name = c.String(),
                        path = c.String(),
                    })
                .PrimaryKey(t => t.folderID)
                .ForeignKey("dbo.schoolClasses", t => t.schoolClassID, cascadeDelete: true)
                .Index(t => t.schoolClassID);
            
            CreateTable(
                "dbo.schoolClasses",
                c => new
                    {
                        schoolClassID = c.Int(nullable: false, identity: true),
                        name = c.String(),
                    })
                .PrimaryKey(t => t.schoolClassID);
            
            CreateTable(
                "dbo.tasks",
                c => new
                    {
                        taskID = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        user_teacherID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.taskID)
                .ForeignKey("dbo.user_teacher", t => t.user_teacherID, cascadeDelete: true)
                .Index(t => t.user_teacherID);
            
            CreateTable(
                "dbo.user_teacher",
                c => new
                    {
                        userID = c.Int(nullable: false, identity: true),
                        schoolClassID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.userID)
                .ForeignKey("dbo.schoolClasses", t => t.schoolClassID, cascadeDelete: true)
                .Index(t => t.schoolClassID);
            
            CreateTable(
                "dbo.folderTypes",
                c => new
                    {
                        folderTypeID = c.Int(nullable: false, identity: true),
                        name = c.String(),
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
                        name = c.String(),
                        room = c.String(),
                        taskID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.scheduleDetailID)
                .ForeignKey("dbo.schedules", t => t.scheduleID, cascadeDelete: true)
                .ForeignKey("dbo.tasks", t => t.taskID, cascadeDelete: true)
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
                .ForeignKey("dbo.schoolClasses", t => t.schoolClassID, cascadeDelete: true)
                .Index(t => t.schoolClassID);
            
            CreateTable(
                "dbo.user_student",
                c => new
                    {
                        userID = c.Int(nullable: false, identity: true),
                        schoolClassID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.userID)
                .ForeignKey("dbo.schoolClasses", t => t.schoolClassID, cascadeDelete: true)
                .Index(t => t.schoolClassID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.user_student", "schoolClassID", "dbo.schoolClasses");
            DropForeignKey("dbo.scheduleDetails", "taskID", "dbo.tasks");
            DropForeignKey("dbo.scheduleDetails", "scheduleID", "dbo.schedules");
            DropForeignKey("dbo.schedules", "schoolClassID", "dbo.schoolClasses");
            DropForeignKey("dbo.files", "taskID", "dbo.tasks");
            DropForeignKey("dbo.tasks", "user_teacherID", "dbo.user_teacher");
            DropForeignKey("dbo.user_teacher", "schoolClassID", "dbo.schoolClasses");
            DropForeignKey("dbo.files", "folderID", "dbo.folders");
            DropForeignKey("dbo.folders", "schoolClassID", "dbo.schoolClasses");
            DropIndex("dbo.user_student", new[] { "schoolClassID" });
            DropIndex("dbo.schedules", new[] { "schoolClassID" });
            DropIndex("dbo.scheduleDetails", new[] { "taskID" });
            DropIndex("dbo.scheduleDetails", new[] { "scheduleID" });
            DropIndex("dbo.user_teacher", new[] { "schoolClassID" });
            DropIndex("dbo.tasks", new[] { "user_teacherID" });
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
