using System;

namespace Core.ViewModels.Admin.Materials
{
    public class AdminMaterialListItemVm
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string ProfessionName { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public string TeacherName { get; set; } = null!;
        public int CommentsCount { get; set; }
        public bool HasFile { get; set; }
        public bool HasExternalUrl { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
