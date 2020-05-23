﻿using IdentityServer.Legacy.Extensions.DependencyInjection;
using IdentityServer.Legacy.Services.Cryptography;
using IdentityServer.Legacy.Services.Serialize;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServer.Legacy.Services.DbContext
{
    public class FileBlobRoleDb : IRoleDbContext, IAdminRoleDbContext
    {
        private string _rootPath = null;
        private ICryptoService _cryptoService = null;
        private IBlobSerializer _blobSerializer;

        public FileBlobRoleDb(IOptions<RoleDbContextConfiguration> options=null)
        {
            if (String.IsNullOrEmpty(options?.Value?.ConnectionString))
                throw new ArgumentException("FileBlobRoleDb: no connection string defined");

            _rootPath = options.Value.ConnectionString;
            _cryptoService = options.Value.CryptoService ?? new Base64CryptoService();
            _blobSerializer = options.Value.BlobSerializer ?? new JsonBlobSerializer();

            DirectoryInfo di = new DirectoryInfo(_rootPath);
            if (!di.Exists)
            {
                di.Create();
            }
        }

        #region IRoleDbContext

        async public Task<IdentityResult> CreateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            role.Id = RolenameToId(role);

            FileInfo fi = new FileInfo($"{ _rootPath }/{ role.Id }.role");

            if (fi.Exists)
            {
                return IdentityResult.Failed(new IdentityError()
                {
                    Code = "already_exists",
                    Description = "Role already exists"
                });
            }

            byte[] buffer = Encoding.UTF8.GetBytes(
                _cryptoService.EncryptText(_blobSerializer.SerializeObject(role)));

            using (var fs = new FileStream(fi.FullName, FileMode.OpenOrCreate,
                            FileAccess.Write, FileShare.None, buffer.Length, true))
            {
                await fs.WriteAsync(buffer, 0, buffer.Length);
            }

            return IdentityResult.Success;
        }

        public Task<IdentityResult> DeleteAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            FileInfo fi = new FileInfo($"{ _rootPath }/{ role.Id }.role");

            if (fi.Exists)
            {
                fi.Delete();
            }

            return Task.FromResult(IdentityResult.Success);
        }

        async public Task<ApplicationRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            FileInfo fi = new FileInfo($"{ _rootPath }/{ roleId }.role");

            if (!fi.Exists)
            {
                return null;
            }

            using (var reader = File.OpenText(fi.FullName))
            {
                var fileText = await reader.ReadToEndAsync();

                fileText = _cryptoService.DecryptText(fileText);

                return _blobSerializer.DeserializeObject<ApplicationRole>(fileText);
            }
        }

        public Task<ApplicationRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return FindByIdAsync(normalizedRoleName.NameToHexId(_cryptoService), cancellationToken);
        }

        async public Task<IdentityResult> UpdateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            FileInfo fi = new FileInfo($"{ _rootPath }/{ role.Id }.role");

            if (!fi.Exists)
            {
                return IdentityResult.Failed(new IdentityError()
                {
                    Code = "not_exists",
                    Description = "Role not exists"
                });
            }
            fi.Delete();

            byte[] buffer = Encoding.UTF8.GetBytes(
                _cryptoService.EncryptText(_blobSerializer.SerializeObject(role)));

            using (var fs = new FileStream(fi.FullName, FileMode.OpenOrCreate,
                            FileAccess.Write, FileShare.None, buffer.Length, true))
            {
                await fs.WriteAsync(buffer, 0, buffer.Length);
            }

            return IdentityResult.Success;
        }

        async public Task<T> UpdatePropertyAsync<T>(ApplicationRole role, string applicationRoleProperty, T propertyValue, CancellationToken cancellation)
        {
            var propertyInfo = role.GetType().GetProperty(applicationRoleProperty);
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(role, propertyValue);

                await UpdateAsync(role, cancellation);
            }

            return propertyValue;
        }

        #endregion

        #region IAdminRoleDbContext

        async public Task<IEnumerable<ApplicationRole>> GetRolesAsync(int limit, int skip, CancellationToken cancellationToken)
        {
            List<ApplicationRole> roles = new List<ApplicationRole>();
            foreach (var fi in new DirectoryInfo(_rootPath).GetFiles("*.role").Skip(skip))
            {
                if (roles.Count >= limit)
                {
                    break;
                }

                using (var reader = File.OpenText(fi.FullName))
                {
                    var fileText = await reader.ReadToEndAsync();

                    fileText = _cryptoService.DecryptText(fileText);

                    roles.Add(_blobSerializer.DeserializeObject<ApplicationRole>(fileText));
                }
            }

            return roles.OrderBy(r => r.Name);
        }

        #endregion

        #region Helper

        private string RolenameToId(ApplicationRole role)
        {
            return role.Name.NameToHexId(_cryptoService);
        }

        #endregion
    }
}
