using Marvin.IDP.DbContexts;
using Marvin.IDP.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marvin.IDP.Services
{
    public class LocalUserService : ILocalUserService
    {
        private readonly IdentityDbContext _identityDbContext;

        public LocalUserService(IdentityDbContext identityDbContext)
        {
            _identityDbContext = identityDbContext;
        }

        public void AddUser(User userToAdd)
        {
            if(userToAdd != null) throw new ArgumentException(nameof(userToAdd));

            if (_identityDbContext.Users.Any(u => u.UserName == userToAdd.UserName)) throw new Exception("UserName must be unique.");

            _identityDbContext.Users.Add(userToAdd);
        }

        public async Task<User> GetUserBySubjectAsyc(string subject)
        {
            if(string.IsNullOrEmpty(subject)) throw new ArgumentNullException(nameof(subject));

            return await _identityDbContext.Users.FirstOrDefaultAsync(u => u.Subject == subject);
        }
                    

        public async Task<bool> ValidateCredentialsAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password)) return false;

            var user = await GetUserByUserNameAsync(username);
            if (user == null || !user.Active) return false;
            
            return (user.Password == password);
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException(nameof(userName));

            return await _identityDbContext.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        }

        public async Task<IEnumerable<UserClaim>> GetUserClaimsBySubjectAsync(string subject)
        {
            if (string.IsNullOrWhiteSpace(subject)) throw new ArgumentNullException(nameof(subject));

            return await _identityDbContext.UserClaims.Where(c => c.User.Subject == subject).ToListAsync();
        }

        public async Task<bool> IsUSerActive(string subject)
        {
            if (string.IsNullOrEmpty(subject)) return false;

            var user = await GetUserBySubjectAsyc(subject);

            if (user == null) return false;
            return user.Active;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _identityDbContext.SaveChangesAsync()>0);
        }
    }
}
