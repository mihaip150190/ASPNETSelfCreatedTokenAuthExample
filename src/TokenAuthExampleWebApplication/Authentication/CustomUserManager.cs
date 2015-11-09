using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using AutoMapper;
using System.Linq;
using Microsoft.Data.Entity;
using AutoMapper.QueryableExtensions;
using TokenAuthExampleWebApplication.AuthModels;

namespace TokenAuthExampleWebApplication.Authentication
{
    public class CustomUserManager<TUser> : IUserManager<TUser> where TUser : BasicUser
    {
        private AuthContext _context;
        private IPasswordHasher<TUser> _hasher;

        public CustomUserManager(AuthContext context, IPasswordHasher<TUser> passwordHasher){
            _context = context;
            _hasher = passwordHasher;
        }

        public bool CheckPassword(TUser user, string password)
        {
            if (user == null)
            {
                return false;
            }

            var result = _hasher.VerifyHashedPassword(user, user.Password, password);

            var success = result != PasswordVerificationResult.Failed;
            //Log Failed event
            return success;
        }

        public async Task<IdentityResult> CreateAsync(TUser user, string password)
        {
            try
            {
                if (user == null)
                {
                    throw new ArgumentNullException("user");
                }
                if (password == null)
                {
                    throw new ArgumentNullException("password");
                }

                SetPasswordHash(user, password);
                var newUser = Mapper.Map<User>(user);

                _context.User.Add(newUser);
                await _context.SaveChangesAsync();

                return IdentityResult.Success;
            }
            catch(Exception ex)
            {
                return IdentityResult.Failed(new IdentityError()
                {
                    Description = ex.Message
                });
            }
            
        }

        public async Task<TUser> FindByNameAsync(string userName)
        {
            return await _context.User.Where(r => r.Username == userName).ProjectTo<TUser>().FirstOrDefaultAsync();
        }

        public async Task<string> GetRoleByIdAsync(int roleId)
        {
            return await _context.Role.Where(r => r.RoleID == roleId).Select(s => s.Description).FirstOrDefaultAsync();
        }

        private void SetPasswordHash(TUser user, string password)
        {
            user.Password = _hasher.HashPassword(user, password);
        }
    }
}
