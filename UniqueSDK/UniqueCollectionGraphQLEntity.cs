using System.Text.Json.Serialization;

namespace UniqueSDK
{
    public class UniqueCollectionGraphQLEntity : Collection
    {
        [JsonPropertyName("actions_count")]
        public int ActionsCount { get; set; }

        [JsonPropertyName("attributes_schema")]
        public object AttributesSchema { get; set; }

        [JsonPropertyName("burned")]
        public bool Burned { get; set; }

        [JsonPropertyName("const_chain_schema")]
        public object ConstChainSchema { get; set; }

        [JsonPropertyName("date_of_creation")]
        public int? DateOfCreation { get; set; }

        [JsonPropertyName("holders_count")]
        public int HoldersCount { get; set; }

        [JsonPropertyName("limits_account_ownership")]
        public int? LimitsAccountOwnership { get; set; }

        [JsonPropertyName("limits_sponsore_data_rate")]
        public float? LimitsSponsoreDataRate { get; set; }

        [JsonPropertyName("limits_sponsore_data_size")]
        public float? LimitsSponsoreDataSize { get; set; }

        [JsonPropertyName("mint_mode")]
        public bool MintMode { get; set; }

        [JsonPropertyName("mode")]
        public string Mode { get; set; }

        [JsonPropertyName("nesting_enabled")]
        public bool NestingEnabled { get; set; }

        [JsonPropertyName("offchain_schema")]
        public string OffchainSchema { get; set; }

        [JsonPropertyName("owner_can_destroy")]
        public bool? OwnerCanDestroy { get; set; }

        [JsonPropertyName("owner_can_transfer")]
        public bool? OwnerCanTransfer { get; set; }

        [JsonPropertyName("owner_normalized")]
        public string OwnerNormalized { get; set; }

        [JsonPropertyName("permissions")]
        public object Permissions { get; set; }

        [JsonPropertyName("schema_version")]
        public string SchemaVersion { get; set; }

        [JsonPropertyName("sponsorship")]
        public string Sponsorship { get; set; }

        [JsonPropertyName("token_limit")]
        public float TokenLimit { get; set; }

        [JsonPropertyName("token_prefix")]
        public string TokenPrefix { get; set; }

        [JsonPropertyName("token_property_permissions")]
        public object TokenPropertyPermissions { get; set; }

        [JsonPropertyName("tokens_count")]
        public int TokensCount { get; set; }

        [JsonPropertyName("transfers_count")]
        public int TransfersCount { get; set; }

        [JsonPropertyName("variable_on_chain_schema")]
        public object VariableOnChainSchema { get; set; }
    }
}

