using System.Text.Json.Serialization;

namespace UniqueSDK
{
	public class UniqueNftGraphQLEntity : Nft
	{
        [JsonPropertyName("amount")]
        public string Amount { get; set; }

        [JsonPropertyName("attributes")]
        public object Attributes { get; set; }

        [JsonPropertyName("bundle_created")]
        public int? BundleCreated { get; set; }

        [JsonPropertyName("burned")]
        public bool Burned { get; set; }

        [JsonPropertyName("children_count")]
        public int? ChildrenCount { get; set; }

        [JsonPropertyName("collection")]
        public UniqueCollectionGraphQLEntity Collection { get; set; }

        [JsonPropertyName("collection_cover")]
        public string CollectionCover { get; set; }

        [JsonPropertyName("collection_description")]
        public string CollectionDescription { get; set; }

        [JsonPropertyName("collection_name")]
        public string CollectionName { get; set; }

        [JsonPropertyName("collection_owner")]
        public string CollectionOwner { get; set; }

        [JsonPropertyName("collection_owner_normalized")]
        public string CollectionOwnerNormalized { get; set; }

        [JsonPropertyName("date_of_creation")]
        public int? DateOfCreation { get; set; }

        private UniqueImageGraplQLEntity image;
        [JsonPropertyName("image")]
        public UniqueImageGraplQLEntity Image { get => image; set { image = value; ImageSource = value.FullUrl; } }

        [JsonPropertyName("is_sold")]
        public bool IsSold { get; set; }

        [JsonPropertyName("nested")]
        public bool Nested { get; set; }

        [JsonPropertyName("owner_normalized")]
        public string OwnerNormalized { get; set; }

        [JsonPropertyName("parent_id")]
        public string ParentId { get; set; }

        [JsonPropertyName("properties")]
        public object Properties { get; set; }

        [JsonPropertyName("token_prefix")]
        public string TokenPrefix { get; set; }

        [JsonPropertyName("tokensOwners")]
        public UniqueTokensOwnersGraphQLEntity TokensOwners { get; set; }

        [JsonPropertyName("tokens_amount")]
        public string TokensAmount { get; set; }

        [JsonPropertyName("tokens_children")]
        public List<object> TokensChildren { get; set; }

        [JsonPropertyName("tokens_owner")]
        public string TokensOwner { get; set; }

        [JsonPropertyName("tokens_parent")]
        public string TokensParent { get; set; }

        [JsonPropertyName("total_pieces")]
        public string TotalPieces { get; set; }

        [JsonPropertyName("transfers_count")]
        public int? TransfersCount { get; set; }

        [JsonPropertyName("type")]
        public TokenTypeEnum Type { get; set; }
    }


    public class UniqueTokensOwnersGraphQLEntity
    {
        [JsonPropertyName("amount")]
        public string Amount { get; set; }

        [JsonPropertyName("collection_id")]
        public int CollectionId { get; set; }

        [JsonPropertyName("date_created")]
        public string DateCreated { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("owner")]
        public string Owner { get; set; }

        [JsonPropertyName("owner_normalized")]
        public string OwnerNormalized { get; set; }

        [JsonPropertyName("token_id")]
        public int TokenId { get; set; }
    }

    public class UniqueImageGraplQLEntity
    {
        [JsonPropertyName("fullUrl")]
        public string FullUrl { get; set; }

        [JsonPropertyName("ipfsCid")]
        public string IpfsCid { get; set; }
    }

    public enum TokenTypeEnum
    {
        FRACTIONAL,
        NFT,
        RFT
    }
}

