﻿using System.Text.Json.Serialization;

namespace Mmicovic.RPSSL.API.Models
{
    public class Credentials
    {
        [JsonPropertyName("name")]
        public string? UserName { get; init; }

        [JsonPropertyName("passphrase")]
        public string? Password { get; init; }
    }
}