using System.ComponentModel.DataAnnotations;

namespace MyAspNetCoreApp.ViewModels
{
    public class UserManagementViewModel
    {
        public string Id { get; set; } = string.Empty;
        
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;
        
        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set; } = string.Empty;
        
        [Display(Name = "Email đã xác thực")]
        public bool EmailConfirmed { get; set; }
        
        [Display(Name = "Khóa đến")]
        public DateTimeOffset? LockoutEnd { get; set; }
        
        [Display(Name = "Vai trò")]
        public List<string> Roles { get; set; } = new();
        
        [Display(Name = "Trạng thái")]
        public string Status => LockoutEnd.HasValue && LockoutEnd > DateTimeOffset.Now ? "Bị khóa" : "Hoạt động";
    }

    public class ManageUserRolesViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public List<string> UserRoles { get; set; } = new();
        public List<string> AllRoles { get; set; } = new();
        public List<string> SelectedRoles { get; set; } = new();
    }
}
