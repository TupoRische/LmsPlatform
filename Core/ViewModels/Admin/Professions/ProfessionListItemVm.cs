namespace Core.ViewModels.Admin.Professions
{
    public class ProfessionListItemVm
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string SchoolName { get; set; } = null!;
        public string? Description { get; set; }
        public int MaterialsCount { get; set; }
    }
}
