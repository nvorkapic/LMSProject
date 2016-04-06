using LMSProject.DataAccess;
using LMSProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMSProject.Controllers
{
    public class TeacherStudentViewModel
    {
        public string id { get; set; }
        public string Name { get; set; }
    }

    public class TeacherClassViewModel
    {
        public string Name { get; set; }
        public int id { get; set; }
        public List<TeacherStudentViewModel> Students { get; set; }
    }
    public class TeacherPrivateFileViewModel
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string  TaskName { get; set; }
    }
    public class TeacherPrivateFolderViewModel
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public List<TeacherPrivateFileViewModel> Files { get; set; }
    }
    public class TeacherViewModel
    {
        public List<TeacherClassViewModel> Classes { get; set; }
        public List<TeacherPrivateFolderViewModel> PrivateFolders { get; set; }
        //public List<TeacherPrivateFileViewModel> PrivateFiles { get; set; }
    }


    [Authorize(Roles ="Teacher")]
    public class TeacherController : Controller
    {
        private LMSContext db = new LMSContext();
        //private ApplicationDbContext dbUser = new ApplicationDbContext();
        private TeacherStudentRepository userRepository = new TeacherStudentRepository();

        // GET: Teacher
        public ActionResult Index()
        {
            TeacherViewModel viewModel = new TeacherViewModel() { Classes = new List<TeacherClassViewModel>() };
            viewModel.PrivateFolders = new List<TeacherPrivateFolderViewModel>();
            //viewModel.PrivateFiles = new List<TeacherPrivateFileViewModel>();

            string TeacherUserId = userRepository.GetUserIdByName(User.Identity.Name);

            var classes = from c in db.users
                          where c.UserId == TeacherUserId
                          select c.schoolClasses;

            foreach (var c in classes)
            {
                List<TeacherStudentViewModel> students = new List<TeacherStudentViewModel>();
                var classStudents = from s in db.users
                         where s.schoolClassID == c.schoolClassID
                         select s;

                foreach(var s in classStudents)
                {
                    students.Add(new TeacherStudentViewModel { id = s.UserId, Name = userRepository.getUserDetailViewModel(s.UserId).UserName });
                }

                viewModel.Classes.Add(new TeacherClassViewModel { id = c.schoolClassID, Name = c.name, Students = students });
            }


            var myfileSelectList = from fil in db.files
                                                 where fil.userID == TeacherUserId && fil.folders.folderTypeID == 1
                                                 select fil;

            foreach(var f in myfileSelectList)
            {
                TeacherPrivateFolderViewModel foundFolder = new TeacherPrivateFolderViewModel {
                    Name = f.folders.name,
                    Path = f.folders.path,
                    Files = new List<TeacherPrivateFileViewModel>()
                };

                if (!viewModel.PrivateFolders.Contains(foundFolder))
                {
                    viewModel.PrivateFolders.Add(foundFolder);
                }

                TeacherPrivateFolderViewModel existingFolder = viewModel.PrivateFolders.Find(p => p.Name == f.name && p.Path == f.path);
                string tempTaskName = "";
                if (f.tasks != null)
                {
                    tempTaskName = f.tasks.name;
                }

                foundFolder.Files.Add(
                    new TeacherPrivateFileViewModel
                    {
                        Name = f.name,
                        Path = f.path,
                        TaskName = tempTaskName
                    });
            }
            return View(viewModel);
        }
    }
}