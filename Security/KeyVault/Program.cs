using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using Azure.Security.KeyVault.Secrets;

namespace KeyVault
{
    /*
     * Prerequisites
     *  - register an app to AAD (Azure Actvive Directory), generate a secret and
     *    and retrieve ClientId, TenantId and the ClientSecret
     *  - create the resource AzureKeyVault and add the AccessPolicy to the 
     *    previous created app on AA
     */
    public class Program
    {
        private const string TENNANT_ID = "";
        private const string CLIENT_ID = "";
        private const string CLIENT_SECRET = "";

        private const string KEY_VAULT_URL = "";

        static async Task Main(string[] args)
        {
            try
            {
                await GetSecretFromKeyVault();
                await EncryptValueWithKey();

                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to complete the operation: {ex.Message}");
            }
        }

        private static async Task GetSecretFromKeyVault()
        {
            const string SECRET_NAME = "KeyVaultTestSecret";
            Console.WriteLine($"Trying to retrieve the value of the secret with name <{SECRET_NAME}>");

            var clientCredentials = GetClientSecretCredential();
            var secretClient = new SecretClient(new Uri(KEY_VAULT_URL), clientCredentials);

            var secretValue = await secretClient
                .GetSecretAsync(SECRET_NAME);

            Console.WriteLine($"The retrieved value is : <{secretValue.Value.Value}>");
        }

        private static async Task EncryptValueWithKey()
        {
            const string KEY_NAME = "KeyVaultEncryptKey";
            const string phraseToEncrypt = "pippoPlutoToEncrypt";

            var clientCredentials = GetClientSecretCredential();
            var keyClient = new KeyClient(new Uri(KEY_VAULT_URL), clientCredentials);

            var encryptionKey = await keyClient
                .GetKeyAsync(KEY_NAME);

            var encryptionClient = new CryptographyClient(encryptionKey.Value.Id, clientCredentials);


            var phraseToEncryptInBytes = Encoding.UTF8.GetBytes(phraseToEncrypt);
            var encryptedPhrase = await encryptionClient
                .EncryptAsync(EncryptionAlgorithm.RsaOaep, phraseToEncryptInBytes);

            Console.WriteLine($"The encrypted text:\n {Convert.ToBase64String(encryptedPhrase.Ciphertext)}");

            var decryptedText = await encryptionClient
                .DecryptAsync(EncryptionAlgorithm.RsaOaep, encryptedPhrase.Ciphertext);
            Console.WriteLine($"The deencrypted text:\n {Encoding.UTF8.GetString(decryptedText.Plaintext)}");
        }

        private static ClientSecretCredential GetClientSecretCredential()
        {
            return new ClientSecretCredential(TENNANT_ID, CLIENT_ID, CLIENT_SECRET);
        }
    }
}
