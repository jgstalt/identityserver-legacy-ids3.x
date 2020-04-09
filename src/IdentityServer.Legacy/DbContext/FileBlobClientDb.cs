﻿using IdentityServer.Legacy.Cryptography;
using IdentityServer.Legacy.DependencyInjection;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Legacy.DbContext
{
    public class FileBlobClientDb : IClientDbContext, IClientDbContextModify
    {
        private string _rootPath = null;
        private ICryptoService _cryptoService = null;

        public FileBlobClientDb(IOptions<ClientDbContextConfiguration> options)
        {
            if (String.IsNullOrEmpty(options?.Value?.ConnectionString))
                throw new ArgumentException("FileBlobClientDb: no connection string defined");

            _rootPath = options.Value.ConnectionString;
            _cryptoService = options.Value.CryptoService ?? new Base64CryptoService();

            DirectoryInfo di = new DirectoryInfo(_rootPath);
            if (!di.Exists)
            {
                di.Create();

                // Initialize Api Clients
                if(options.Value.IntialClients!=null)
                {
                    foreach(var client in options.Value.IntialClients)
                    {
                        AddClientAsync(client).Wait();
                    }
                }
            }
        }

        #region IClientDbContext

        async public Task<Client> FindClientByIdAsync(string clientId)
        {
            FileInfo fi = new FileInfo($"{ _rootPath }/{ clientId.NameToHexId(_cryptoService) }.client");

            if(!fi.Exists)
            {
                return null;
            }

            using (var reader = File.OpenText(fi.FullName))
            {
                var fileText = await reader.ReadToEndAsync();
                fileText = _cryptoService.DecryptText(fileText);

                return JsonConvert.DeserializeObject<Client>(fileText);
            }
        }

        #endregion

        #region IClientDbContextModify

        async public Task AddClientAsync(Client client)
        {
            string id = client.ClientId.NameToHexId(_cryptoService);
            FileInfo fi = new FileInfo($"{ _rootPath }/{ id }.client");

            if (fi.Exists)
            {
                throw new Exception("Client already exists");
            }

            byte[] buffer = Encoding.UTF8.GetBytes(
                _cryptoService.EncryptText(JsonConvert.SerializeObject(client)));

            using (var fs = new FileStream(fi.FullName, FileMode.OpenOrCreate,
                            FileAccess.Write, FileShare.None, buffer.Length, true))
            {
                await fs.WriteAsync(buffer, 0, buffer.Length);
            }
        }

        public Task RemoveClientAsync(Client client)
        {
            FileInfo fi = new FileInfo($"{ _rootPath }/{ client.ClientId.NameToHexId(_cryptoService) }.client");

            if (fi.Exists)
            {
                fi.Delete();
            }

            return Task.CompletedTask;
        }

        async public Task UpdateClientAsync(Client client)
        {
            FileInfo fi = new FileInfo($"{ _rootPath }/{ client.ClientId.NameToHexId(_cryptoService) }.client");

            if (fi.Exists)
            {
                fi.Delete();
            }

            await AddClientAsync(client);
        }

        async public Task<IEnumerable<Client>> GetAllClients()
        {
            List<Client> clients = new List<Client>();

            foreach(var fi in new DirectoryInfo(_rootPath).GetFiles("*.client"))
            {
                using (var reader = File.OpenText(fi.FullName))
                {
                    var fileText = await reader.ReadToEndAsync();
                    fileText = _cryptoService.DecryptText(fileText);

                    clients.Add(JsonConvert.DeserializeObject<Client>(fileText));
                }
            }

            return clients;
        }

        #endregion
    }
}
