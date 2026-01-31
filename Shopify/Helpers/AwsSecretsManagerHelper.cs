
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

namespace Shopify.Helpers;

public class AwsSecretsManagerHelper(IAmazonSecretsManager client)
{
    public async Task<string> GetSecretAsync(string secretName)
    {
        var response = await client.GetSecretValueAsync(new GetSecretValueRequest
        {
            SecretId = secretName
        });

        return response.SecretString;
    }
}
