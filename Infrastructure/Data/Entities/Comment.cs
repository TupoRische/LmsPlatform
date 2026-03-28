using Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities
{
    public class Comment
    {
        public int Id { get; set; }

        [Required, MaxLength(1000)]
        public string Content { get; set; } = null!;

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        // FK към Material
        public int MaterialId { get; set; }
        public Material Material { get; set; } = null!;

        // FK към User (кой коментира)
        [Required]
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
<<<<<<< HEAD

        // Reply / threading support
        public int? ParentCommentId { get; set; }
        public Comment? ParentComment { get; set; }
        public ICollection<Comment> Replies { get; set; } = new HashSet<Comment>();
=======
>>>>>>> b1645c236beb100f9b792702ab7ac3ba0a399b56
    }

}
