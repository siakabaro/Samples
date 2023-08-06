using Microsoft.IdentityModel.Tokens;

namespace GenerateRsaJwk.Enums
{
    /// <summary>
    /// Json Web Encryption (JWE) and Json Web Signature (JWE) algorithms that use RSA
    /// RFC-7518
    /// </summary>
    public enum RsaAlgorithm
    {
        /// <summary>
        // No algorithm
        /// </summary>
        None,

        /// <summary>
        /// Signature algorithm RSASSA-PKCS1-v1_5 using SHA-256
        /// </summary>
        [JsonWebAlgorithm(SecurityAlgorithms.RsaSha256, IsSignatureAlgorithm = true)]
        Rs256,

        /// <summary>
        /// Signature algorithm RSASSA-PKCS1-v1_5 using SHA-384
        /// </summary>
        [JsonWebAlgorithm(SecurityAlgorithms.RsaSha384, IsSignatureAlgorithm = true)]
        Rs384,

        /// <summary>
        /// Signature algorithm RSASSA-PKCS1-v1_5 using SHA-512
        /// </summary>
        [JsonWebAlgorithm(SecurityAlgorithms.RsaSha512, IsSignatureAlgorithm = true)]
        Rs512,

        /// <summary>
        /// Signature algorithm RSASSA-PSS using SHA-256 and MGF1 with SHA-256
        /// </summary>
        [JsonWebAlgorithm(SecurityAlgorithms.RsaSsaPssSha256, IsSignatureAlgorithm = true)]
        Ps256,

        /// <summary>
        /// Signature algorithm RSASSA-PSS using SHA-256 and MGF1 with SHA-384
        /// </summary>
        [JsonWebAlgorithm(SecurityAlgorithms.RsaSsaPssSha384, IsSignatureAlgorithm = true)]
        Ps384,

        /// <summary>
        /// Signature algorithm RSASSA-PSS using SHA-256 and MGF1 with SHA-512
        /// </summary>
        [JsonWebAlgorithm(SecurityAlgorithms.RsaSsaPssSha512, IsSignatureAlgorithm = true)]
        Ps512,

        /// <summary>
        ///Encryption algorithm RSAES-PKCS1-v1_5
        /// </summary>
        [JsonWebAlgorithm(SecurityAlgorithms.RsaPKCS1, IsSignatureAlgorithm = false)]
        Rsa1_5,

        /// <summary>
        /// Encryption algorithm RSAES OAEP using default parameters
        /// </summary>
        [JsonWebAlgorithm(SecurityAlgorithms.RsaOAEP, IsSignatureAlgorithm = false)]
        RsaOaep,

        /// <summary>
        /// Encryption algorithm RSAES OAEP using SHA-256 and MGF1 with SHA-256
        /// </summary>
        [JsonWebAlgorithm("RSA-OAEP-256", IsSignatureAlgorithm = false)]
        RsaOaep256,

        /// <summary>
        /// Encryption algorithm RSAES OAEP using SHA-384 and MGF1 with SHA-384
        /// </summary>
        [JsonWebAlgorithm("RSA-OAEP-384", IsSignatureAlgorithm = false)]
        RsaOaep384,

        /// <summary>
        /// Encryption algorithm RSAES OAEP using SHA-512 and MGF1 with SHA-512
        /// </summary>
        [JsonWebAlgorithm("RSA-OAEP-512", IsSignatureAlgorithm = false)]
        RsaOaep512,

    }
}
