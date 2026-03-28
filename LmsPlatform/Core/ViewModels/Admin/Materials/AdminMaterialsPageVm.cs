using System.Collections.Generic;

namespace Core.ViewModels.Admin.Materials
{
    public class AdminMaterialsPageVm
    {
        public int TotalMaterials { get; set; }
        public int WithFilesCount { get; set; }
        public int WithExternalLinksCount { get; set; }
        public int TotalComments { get; set; }
        public IEnumerable<AdminMaterialListItemVm> Materials { get; set; } = new List<AdminMaterialListItemVm>();
    }
}
