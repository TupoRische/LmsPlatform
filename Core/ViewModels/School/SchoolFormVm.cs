using System.ComponentModel.DataAnnotations;

namespace Core.ViewModels.School
{
    public class SchoolFormVm
    {
        [Required, MaxLength(200)]
        public string Name { get; set; } = null!;

        [Required, MaxLength(100)]
        public string City { get; set; } = null!;

        [MaxLength(2000)]
        public string? Description { get; set; }
    }
}
