using Marvin.IDP.Entities;

namespace Marvin.IDP.Services
{
    public interface ILocalUserService
    {
        Task<bool> ValidateCredentialsAsync(string username, string password);

        Task<IEnumerable<UserClaim>> GetUserClaimsBySubjectAsync(string subject);

        Task<User> GetUserByUserNameAsync(string userName);

        Task<User> GetUserBySubjectAsyc(string subject);

        void AddUser(User userToAdd, string password);

        Task<bool> IsUSerActive(string subject);

        Task<bool> SaveChangesAsync();
        Task<bool> ActivateUserAsync(string securityCode);
    }
}
