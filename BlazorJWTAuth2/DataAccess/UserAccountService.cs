namespace BlazorJWTAuth2.DataAccess
{
    public class UserAccountService
    {
        private static readonly UserAccount[] Accounts = new UserAccount[]
        {
            new UserAccount(){ UId= 0, UserId = "sa", UserName = "최고 관리자" , DepartmentText = "", TeamText = "" },
            new UserAccount(){ UId= 1, UserId = "admin", UserName = "관리자", DepartmentText = "", TeamText = "" },
            new UserAccount(){ UId= 2, UserId = "guest", UserName = "손님", DepartmentText = "", TeamText = "" },
        };


        public Task<UserAccount?> GetAccountsAsync(string UserId)
        {
            return Task.FromResult(Accounts.FirstOrDefault(x => x.UserId == UserId));
        }
    }
}