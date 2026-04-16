namespace Core.ViewModels.School
{
    public class SchoolListVm
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string City { get; set; } = null!;
        public int ProfessionsCount { get; set; }
        public int UsersCount { get; set; }
    }

}
