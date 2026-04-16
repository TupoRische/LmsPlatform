using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.Home
{
    public class SearchResultVm
    {
        public int Id { get; set; } 
        public string Title { get; set; } = null!;
        public string Type { get; set; } = null!; // "Profession" или "School"
        public string Description { get; set; } = null!;
        public string Url { get; set; } = null!;
    }
}
