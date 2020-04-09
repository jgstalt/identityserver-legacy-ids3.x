﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServer.Legacy.DbContext
{
    public interface IUserDbContext
    {
        Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken);
        Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken);
        Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken);
        Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken);
        Task<ApplicationUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken); 
        Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken);
        Task<T> UpdatePropertyAsync<T>(ApplicationUser user, string applicationUserProperty, T propertyValue, CancellationToken cancellation);
    }
}
