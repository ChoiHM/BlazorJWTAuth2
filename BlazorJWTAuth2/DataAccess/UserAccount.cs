using System.ComponentModel.DataAnnotations;

namespace BlazorJWTAuth2.DataAccess
{
    public class UserAccount
    {
        public int UId { get; set; }
        public string UserId { get; set; }
        public string? UserName { get; set; }
        public string? DepartmentText { get; set; }
        public string? TeamText { get; set; }

    }
}