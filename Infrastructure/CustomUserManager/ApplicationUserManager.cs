using Domain.Entities;
using Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.CustomUserManager
{
    public class ApplicationUserManager : UserManager<User>
    {
        private readonly UserStore<User, Role, ApplicationDbContext, string, IdentityUserClaim<string>,
          IdentityUserRole<string>, IdentityUserLogin<string>, IdentityUserToken<string>, IdentityRoleClaim<string>>
          _store;

        public ApplicationUserManager(
        IUserStore<User> store,
        IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<User> passwordHasher,
        IEnumerable<IUserValidator<User>> userValidators,
        IEnumerable<IPasswordValidator<User>> passwordValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<UserManager<User>> logger)
        : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _store = (UserStore<User, Role, ApplicationDbContext, string, IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, IdentityUserToken<string>, IdentityRoleClaim<string>>)store;
        }

        public virtual async Task<IdentityResult> AddToRoleByRoleIdAsync(User user, string roleId)
        {
            ThrowIfDisposed();

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrWhiteSpace(roleId))
                throw new ArgumentNullException(nameof(roleId));

            if (await IsInRoleByIdAsync(user, roleId, CancellationToken))
                return IdentityResult.Failed(ErrorDescriber.UserAlreadyInRole(roleId));

            _store.Context.Set<IdentityUserRole<string>>().Add(new IdentityUserRole<string> { RoleId = roleId, UserId = user.Id });

            return await UpdateUserAsync(user);
        }

        public async Task<bool> IsInRoleByIdAsync(User user, string roleId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrWhiteSpace(roleId))
                throw new ArgumentNullException(nameof(roleId));

            var role = await _store.Context.Set<Role>().FindAsync(roleId);
            if (role == null)
                return false;

            var userRole = await _store.Context.Set<IdentityUserRole<string>>().FindAsync(new object[] { user.Id, roleId }, cancellationToken);
            return userRole != null;
        }
    }
}
