using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.Comments
{
    public class CommentFormVm
    {
        [Required, MaxLength(1000)]
        public string Content { get; set; } = null!;

        public int? ParentCommentId { get; set; }
    }
}
