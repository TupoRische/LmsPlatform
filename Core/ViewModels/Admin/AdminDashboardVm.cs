using Core.ViewModels.Admin.Professions;
using Core.ViewModels.Admin.Schools;
using Core.ViewModels.Admin.Students;
using Core.ViewModels.Admin.Teachers;
using Core.ViewModels.Admin.Users;
<<<<<<< HEAD
using Core.ViewModels.Admin.Materials;
using Core.ViewModels.Comments;
=======
>>>>>>> b1645c236beb100f9b792702ab7ac3ba0a399b56
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.Admin
{
    public class AdminDashboardVm
    {
        public int TotalUsers { get; set; }
        public int Teachers { get; set; }
        public int Students { get; set; }
        public int PendingTeachers { get; set; }

        public int Schools { get; set; }
        public int Professions { get; set; }
        public int Materials { get; set; }
        public int Comments { get; set; }
        public List<SimpleUserVm> LatestPendingTeachers { get; set; } = new();
        public List<SimpleUserVm> LatestTeachers { get; set; } = new();
        public List<SimpleUserVm> LatestStudents { get; set; } = new();
        public int UsersCount { get; set; }
    public int TeachersCount { get; set; }
    public int StudentsCount { get; set; }

        public IEnumerable<RecentUserVm> RecentUsers { get; set; } = new List<RecentUserVm>();
        public IEnumerable<RandomTeacherVm> RandomTeachers { get; set; } = new List<RandomTeacherVm>();
        public IEnumerable<RandomStudentVm> RandomStudents { get; set; }
    = new List<RandomStudentVm>();
        public IEnumerable<PendingTeacherVm> PendingTeachersPreview { get; set; }
    = new List<PendingTeacherVm>();
        public IEnumerable<RandomSchoolVm> RandomSchools { get; set; }
    = new List<RandomSchoolVm>();
        public IEnumerable<RandomProfessionVm> RandomProfessions { get; set; }
    = new List<RandomProfessionVm>();
<<<<<<< HEAD
        public IEnumerable<RandomMaterialVm> RandomMaterials { get; set; }
    = new List<RandomMaterialVm>();
        public IEnumerable<CommentThreadVm> RandomCommentThreads { get; set; }
    = new List<CommentThreadVm>();
=======
>>>>>>> b1645c236beb100f9b792702ab7ac3ba0a399b56
    }
}
