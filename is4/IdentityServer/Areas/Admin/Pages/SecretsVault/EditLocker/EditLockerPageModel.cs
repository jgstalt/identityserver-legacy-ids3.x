﻿using IdentityServer.Legacy;
using IdentityServer.Legacy.DependencyInjection;
using IdentityServer.Legacy.Exceptions;
using IdentityServer.Legacy.Models;
using IdentityServer.Legacy.Services.DbContext;
using IdentityServer.Legacy.UserInteraction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServer.Areas.Admin.Pages.SecretsVault.EditLocker
{
    public class EditLockerPageModel : SecurePageModel, IEditLockerPageModel
    {
        public EditLockerPageModel(
            ISecretsVaultDbContext roleDbContext)
        {
            _secretsVaultDb = roleDbContext;
        }

        protected ISecretsVaultDbContext _secretsVaultDb = null;

        async protected Task LoadCurrentLockerAsync(string id)
        {
            this.CurrentLocker = (await _secretsVaultDb.GetLockersAsync(CancellationToken.None))
                                        .Where(l => l.Name == id)
                                        .FirstOrDefault();
        }

        public SecretsLocker CurrentLocker { get; set; }
    }
}