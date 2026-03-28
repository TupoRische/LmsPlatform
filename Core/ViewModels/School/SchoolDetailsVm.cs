namespace Core.ViewModels.School
{
    public class SchoolDetailsVm
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string City { get; set; } = null!;
        public string? Description { get; set; }
    }

}
