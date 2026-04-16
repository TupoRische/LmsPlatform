using System.Collections.Generic;

namespace Core.ViewModels.Admin.Users
{
    public class AdminUsersPageVm
    {
        public int TotalUsers { get; set; }
        public int AdminsCount { get; set; }
        public int TeachersCount { get; set; }
        public int StudentsCount { get; set; }
        public int PendingTeachersCount { get; set; }
        public IEnumerable<AdminUserListItemVm> Users { get; set; } = new List<AdminUserListItemVm>();
    }
}
