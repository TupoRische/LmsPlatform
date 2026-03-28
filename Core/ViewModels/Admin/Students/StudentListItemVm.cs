using System;

namespace Core.ViewModels.Admin.Students
{
    public class StudentListItemVm
    {
        public string Id { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string SchoolName { get; set; } = null!;
        public bool HasSchool { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CommentsCount { get; set; }
        public int QuizResultsCount { get; set; }
    }
}
