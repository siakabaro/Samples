using GenerateRsaJwk.Enums;
using Jwk.Generator;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using System.Text.Json;
using System.Runtime;
using System.Diagnostics;

namespace GenerateRsaJwk
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("GenerateRsaJwk Console App");
            Console.WriteLine("Generating 2048 RSA Key");
            IRsaKeyGenerator generator = new RsaKeyGenerator();
            RsaSecurityKey key = generator.GenerateKey(2048, "my_key_id");

            Console.WriteLine($"Generated RSA Key with size: {key.KeySize}");

           
            //Use the custom ToJwk extension to convert the RSA Key in JWK object
            var alg = RsaAlgorithm.Rs256;
            JsonWebKey jwk1 = key.ToJwk(alg, true);
            JsonWebKey jwkPublic = key.ToJwk(alg, false);

            // Use JsonWebKeyConverter to convert the RSA Key in JWK object
            JsonWebKey jwk2 = JsonWebKeyConverter.ConvertFromRSASecurityKey(key);
            var algorithmInfo = alg.GetJsonWebAlgorithm();
            jwk2.Use = algorithmInfo?.PublicKeyUse;
            jwk2.Alg = algorithmInfo?.Name;

            //Assertion. JWK.ToString returns the formatted string: GetType(), Use: 'value', Kid: 'value', Kty: 'value', InternalId: 'value'.
            Debug.Assert(jwk1.ToString() == jwk2.ToString());

            //output the JWK string
            ConvertToStringWithJsonExtensions1(jwk1, jwkPublic);
            ConvertToStringWithJsonExtensions2(jwk1, jwkPublic);
            ConvertToStringWithJsonSerializer(jwk1, jwkPublic);
            ConvertToStringManually(jwk1, jwkPublic);

            Console.ReadLine();
        }

        static void ConvertToStringManually(JsonWebKey jwk, JsonWebKey jwkPublic)
        {
            Console.WriteLine($"------------------------------------------------------------");
            Console.WriteLine($"Serialize the JWK to String with custom classes (Manually)");
            Console.WriteLine($"JWK:");
            Console.WriteLine(jwk.SerializeToJson());
            Console.WriteLine();
            Console.WriteLine($"JWK (public key):");
            Console.WriteLine(jwkPublic.SerializeToJson());
            Console.WriteLine();
        }
        static void ConvertToStringWithJsonExtensions1(JsonWebKey jwk, JsonWebKey jwkPublic)
        {
            Console.WriteLine($"------------------------------------------------------------");
            Console.WriteLine($"Serialize the JWK to String with JsonExtensions.SerializeToJson");
            Console.WriteLine($"JWK Raw:");
            Console.WriteLine(JsonExtensions.SerializeToJson(jwk));
            Console.WriteLine();
            Console.WriteLine($"JWK (public key):");
            Console.WriteLine(JsonExtensions.SerializeToJson(jwkPublic));
            Console.WriteLine();
        }

        static void ConvertToStringWithJsonExtensions2(JsonWebKey jwk, JsonWebKey jwkPublic)
        {
            Console.WriteLine($"------------------------------------------------------------");
            Console.WriteLine($"Serialize the JWK to String with JsonExtensions.SerializeToJson and indent the output");
            Console.WriteLine($"JWK Raw:");
            var jwkString = JsonExtensions.SerializeToJson(jwk);
            Console.WriteLine(JValue.Parse(jwkString).ToString(Formatting.Indented));
            Console.WriteLine();
            Console.WriteLine($"JWK (public key):");
            var jwkPublicString = JsonExtensions.SerializeToJson(jwkPublic);
            Console.WriteLine(JValue.Parse(jwkPublicString).ToString(Formatting.Indented));
            Console.WriteLine();
        }
        static void ConvertToStringWithJsonSerializer(JsonWebKey jwk, JsonWebKey jwkPublic)
        {
            Console.WriteLine($"------------------------------------------------------------");
            Console.WriteLine($"Serialize the JWK to String with System.Text.Json.JsonSerializer.Serialize");
            Console.WriteLine($"JWK Raw:");
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jwkString = System.Text.Json.JsonSerializer.Serialize(jwk, options);
            Console.WriteLine(jwkString);
            Console.WriteLine();
            Console.WriteLine($"JWK (public key):");
            var jwkPublicString = System.Text.Json.JsonSerializer.Serialize(jwkPublic,options);
            Console.WriteLine(JValue.Parse(jwkPublicString).ToString(Formatting.Indented));
            Console.WriteLine();
        }
    }
}