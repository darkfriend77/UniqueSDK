using System;
using System.Text.Json.Serialization;

namespace UniqueSDK
{
    /// <summary>
    /// Unified Nft object
    /// </summary>
	public class Nft
	{
        /// <summary>
        /// Collection ID
        /// </summary>
        [JsonPropertyName("collection_id")]
        public int CollectionId { get; set; }

        /// <summary>
        /// Owner
        /// </summary>
        [JsonPropertyName("owner")]
        public string Owner { get; set; }

        /// <summary>
        /// Token ID
        /// </summary>
        [JsonPropertyName("token_id")]
        public int TokenId { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [JsonPropertyName("token_name")]
        public string TokenName { get; set; }

        /// <summary>
        /// Image
        /// </summary>
        [JsonPropertyName("image_source")]
        public string ImageSource { get; set; }

        /// <summary>
        /// Implicitely convert unified Collection to UniqueCollectionRest type
        /// </summary>
        /// <param name="p"></param>
        public static implicit operator UniqueNftRest(Nft p) => new UniqueNftRest
        {
            CollectionId = p.CollectionId,
            Name = p.TokenName,

            // Owner
            Address = p.Owner,
            Owner = p.Owner,
            Image = p.ImageSource,
        };
    }
}

