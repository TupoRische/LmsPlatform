using System;

namespace Core.ViewModels.Admin.Users
{
    public class AdminUserListItemVm
    {
        public string Id { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string SchoolName { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public bool IsApproved { get; set; }
    }
}
