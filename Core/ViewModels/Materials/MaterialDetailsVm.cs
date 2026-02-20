using Core.ViewModels.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.Materials
{
    public class MaterialDetailsVm
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public string? FilePath { get; set; }
        public string? Url { get; set; }

        public string ProfessionName { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public string TeacherName { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public IEnumerable<CommentListVm> Comments { get; set; } = new List<CommentListVm>();
    }
}
