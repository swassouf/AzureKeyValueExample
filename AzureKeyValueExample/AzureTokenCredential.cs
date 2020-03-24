using Azure.Core;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AzureKeyValueExample
{
    public class AzureTokenCredential: TokenCredential
    {
        private readonly IConfidentialClientApplication _publicClientApp;

        public AzureTokenCredential()
        {
            _publicClientApp = ConfidentialClientApplicationBuilder.Create("e3a2c1c6-39dd-49f4-a507-06168f01ece6")
                 .WithClientSecret("2=6ZEIU:[0LLPS4HYg[gpeo.kSMDo3jj").WithTenantId("d59273ac-bb0c-4f66-9618-2b5f12cbcfc6")
                   .WithRedirectUri("http://localhost:30808")
                   .Build();
        }

        public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken)
        {
            return GetTokenAsync(requestContext, cancellationToken).GetAwaiter().GetResult();
        }

        public override async ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken)
        {
            return await GetTokenInteractiveAsync(requestContext.Scopes, cancellationToken).ConfigureAwait(false);
        }

        private async Task<AccessToken> GetTokenInteractiveAsync(string[] scopes, CancellationToken cancellationToken)
        {
            var authResult = await _publicClientApp.AcquireTokenForClient(scopes)
                         .ExecuteAsync(cancellationToken);

            return new AccessToken(authResult.AccessToken, authResult.ExpiresOn);
        }
    }
}
