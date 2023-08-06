using Microsoft.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace GenerateRsaJwk
{
    public interface IRsaKeyGenerator
    {
        /// <summary>
        /// Generates a new Rsa Key
        /// </summary>
        /// <param name="keySize">Size of the Rsa Key</param>
        /// <param name="keyId">Id or name of the key</param>
        /// <returns></returns>
        RsaSecurityKey GenerateKey(int keySize, [Optional] string keyId);
    }

    /// <summary>
    /// Default Implementation of IRsaKeyGenerator
    /// </summary>
    public sealed class RsaKeyGenerator : IRsaKeyGenerator
    {
        /// <summary>
        /// Generates a new Rsa Key
        /// </summary>
        /// <param name="keySize">Size of the Rsa Key</param>
        /// <param name="keyId">Id or name of the key</param>
        /// <returns></returns>
        public RsaSecurityKey GenerateKey(int keySize, [Optional] string keyId)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.KeySize = keySize;
                RSAParameters parameters = rsa.ExportParameters(true);
                return new RsaSecurityKey(parameters) { KeyId = String.IsNullOrWhiteSpace(keyId) ? Guid.NewGuid().ToString() : keyId };
        
            }
        }
    }
}
