// Copyright (c) 2020 Siaka Baro. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Xml;

namespace Jwk.Generator
{
    public static class JwkExtensions
    {
        internal static JsonSerializerSettings IndentedSettings = new JsonSerializerSettings()
        {
            Formatting = Newtonsoft.Json.Formatting.Indented,
            ContractResolver = new DefaultContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };

        internal static JsonSerializerSettings Settings = new JsonSerializerSettings()
        {
            Formatting = Newtonsoft.Json.Formatting.None,
            ContractResolver = new DefaultContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };

        public static string SerializeToJson(this JsonWebKey jwk, bool indented = true)
        {
            JsonWebKeySerializationWrapper wrapper = new JsonWebKeySerializationWrapper()
            {
                Kty = jwk.Kty,
                Kid = jwk.Kid,
                Alg = jwk.Alg,
                KeyOps = jwk.KeyOps,
                K = jwk.K,
                E = jwk.E,
                N = jwk.N,
                Oth = jwk.Oth,
                P = jwk.P,
                Q = jwk.Q,
                QI = jwk.QI,
                DQ = jwk.DQ,
                DP = jwk.DP,
                D = jwk.D,
                Crv = jwk.Crv,
                Use = jwk.Use,
                X = jwk.X,
                Y = jwk.Y,
                X5c = jwk.X5c,
                X5t = jwk.X5t,
                X5tS256 = jwk.X5tS256,
                X5u = jwk.X5u
            };
            return JsonConvert.SerializeObject(wrapper, indented ? IndentedSettings : Settings);
        }

        internal class JsonWebKeySerializationWrapper
        {
            [JsonProperty("kty")]
            public string? Kty { get; set; }

            [JsonPropertyAttribute("alg")]
            public string? Alg { get; set; }

            [JsonPropertyAttribute("kid")]
            public string? Kid { get; set; }

            [JsonProperty("use")]
            public string? Use { get; set; }

            [JsonProperty("n")]
            public string? N { get; set; }

            [JsonProperty("e")]
            public string? E { get; set; }

            [JsonProperty("d")]
            public string? D { get; set; }

            [JsonProperty("p")]
            public string? P { get; set; }

            [JsonPropertyAttribute("q")]
            public string? Q { get; set; }

            [JsonProperty("dp")]
            public string? DP { get; set; }

            [JsonProperty("dq")]
            public string? DQ { get; set; }

            [JsonProperty("qi")]
            public string? QI { get; set; }

            [JsonPropertyAttribute("crv")]
            public string? Crv { get; set; }

            [JsonProperty("x")]
            public string? X { get; set; }

            [JsonProperty("y")]
            public string? Y { get; set; }

            [JsonProperty("x5c")]
            public IList<string>? X5c { get; set; }

            [JsonProperty("x5t")]
            public string? X5t { get; set; }

            [JsonProperty("x5t#S256")]
            public string? X5tS256 { get; set; }

            [JsonPropertyAttribute("x5u")]
            public string? X5u { get; set; }

            [JsonProperty("k")]
            public string? K { get; set; }

            [JsonPropertyAttribute("key_ops")]
            public IList<string>? KeyOps { get; set; }

            [JsonProperty("oth")]
            public IList<string>? Oth { get; set; }

            public bool ShouldSerializeKeyOps()
            {
                return KeyOps?.Count > 0;
            }

            public bool ShouldSerializeX5c()
            {
                return X5c?.Count > 0;
            }
        }
    }
}
