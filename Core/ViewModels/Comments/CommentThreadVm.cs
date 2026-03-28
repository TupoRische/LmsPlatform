using System;

using System.Collections.Generic;

using System.Linq;

using System.Text;

using System.Threading.Tasks;



namespace Core.ViewModels.Comments

{

    public class CommentThreadVm

    {

        public int MaterialId { get; set; }

        public string MaterialTitle { get; set; } = null!;



        public int CommentsCount { get; set; }



        public string LastComment { get; set; } = null!;

        public DateTime LastCommentDate { get; set; }



        public List<string> Participants { get; set; } = new();

    }

}