using Microsoft.IdentityModel.Tokens;

namespace GenerateRsaJwk.Enums
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Enum)]
    public class JsonWebAlgorithmAttribute : Attribute
    {
        public string Name { get; }

        public string PublicKeyUse
        {
            get
            {
                if (IsSignatureAlgorithm)
                    return JsonWebKeyUseNames.Sig;
                return JsonWebKeyUseNames.Enc;
            }
        }

        public bool IsSignatureAlgorithm { get; set; }

        internal JsonWebAlgorithmAttribute(string name)
        {
            Name = name;
        }
    }
}
