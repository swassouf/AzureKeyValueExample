using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AzureKeyValueExample.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace AzureKeyValueExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration  configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var test = _configuration["TestApp:Settings:BackgroundColor"];
            var test2 = _configuration["AppSecretFromKeyVault"];

            // KeyVault  Other azure portal
            var azureServiceProvider = new AzureServiceTokenProvider();
            var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(authenticationCallBack));
            var secret = await keyVaultClient.GetSecretAsync("https://ContosoKeyValue.vault.azure.net/secrets/AppSecret")
                 .ConfigureAwait(false);
            var message = secret.Value;

            return View();
        }

        private async Task<string> authenticationCallBack(string authority, string resource, string scope)
        {
            var clientCredential = new ClientCredential("52d93311-30ba-42c3-ac8c-01c1f3bd8c69", "lax:XeDSKRF-]yAl:dIYGhxMHjok4681");
            var authenticationContext = new AuthenticationContext(authority);
            var accessToken = await authenticationContext.AcquireTokenAsync(resource, clientCredential);
            return accessToken.AccessToken;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
