﻿using IdentityServer.Legacy.Services.Cryptography;
using IdentityServer.Legacy.Services.DbContext;
using IdentityServer.Legacy.Services.Serialize;
using IdentityServer.Legacy.UserInteraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer.Legacy.Extensions.DependencyInjection
{
    public class UserDbContextConfiguration
    {
        public string ConnectionString { get; set; }
        public ICryptoService CryptoService { get; set; }
        public IBlobSerializer BlobSerializer { get; set; }

        public ManageAccountEditor ManageAccountEditor { get; set; }
        public AdminAccountEditor AdminAccountEditor { get; set; }
    }
}
