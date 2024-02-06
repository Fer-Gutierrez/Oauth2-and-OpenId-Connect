using Marvin.IDP.DbContexts;
using Marvin.IDP.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Marvin.IDP.Services
{
    public class LocalUserService : ILocalUserService
    {
        private readonly IdentityDbContext _identityDbContext;
        private readonly IPasswordHasher<User> _passwordHasher;

        public LocalUserService(IdentityDbContext identityDbContext, IPasswordHasher<User> passwordHasher)
        {
            _identityDbContext = identityDbContext ?? throw new ArgumentNullException(nameof(identityDbContext));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        public void AddUser(User userToAdd, string password)
        {
            if(userToAdd == null) throw new ArgumentException(nameof(userToAdd));

            if (_identityDbContext.Users.Any(u => u.UserName == userToAdd.UserName)) throw new Exception("UserName must be unique.");

            if (_identityDbContext.Users.Any(u => u.Email == userToAdd.Email)) throw new Exception("Email must be unique.");

            userToAdd.SecurityCode = Convert.ToBase64String(RandomNumberGenerator.GetBytes(128));
            userToAdd.SecurityCodeExpirationDate = DateTime.UtcNow.AddHours(1);

            userToAdd.Password = _passwordHasher.HashPassword(userToAdd,password);

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
            
            //return (user.Password == password);
            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            return (verificationResult == PasswordVerificationResult.Success);
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


        public async Task<bool> ActivateUserAsync(string securityCode)
        {
            if (string.IsNullOrWhiteSpace(securityCode)) throw new ArgumentNullException(nameof(securityCode));

            var user = await _identityDbContext.Users.FirstOrDefaultAsync(u => 
                u.SecurityCode == securityCode &&
                u.SecurityCodeExpirationDate >= DateTime.UtcNow);

            if (user == null) return false;
            user.Active = true;
            user.SecurityCode = null;
            return user.Active;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _identityDbContext.SaveChangesAsync()>0);
        }
    }
}
