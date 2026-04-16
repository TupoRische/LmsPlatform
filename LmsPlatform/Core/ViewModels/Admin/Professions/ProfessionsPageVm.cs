using System.Collections.Generic;

namespace Core.ViewModels.Admin.Professions
{
    public class ProfessionsPageVm
    {
        public int TotalProfessions { get; set; }
        public int SchoolsRepresented { get; set; }
        public int WithMaterialsCount { get; set; }
        public int TotalMaterials { get; set; }
        public IEnumerable<ProfessionListItemVm> Professions { get; set; } = new List<ProfessionListItemVm>();
    }
}
