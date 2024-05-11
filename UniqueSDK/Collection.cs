using System.Text.Json.Serialization;
using Substrate.NetApi.Model.Types.Primitive;

namespace UniqueSDK
{
	/// <summary>
	/// Unified Collection object
	/// </summary>
	public class Collection
	{
        /// <summary>
        /// Collection ID
        /// </summary>
        [JsonPropertyName("collection_id")]
        public int CollectionId { get; }

        /// <summary>
        /// Name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// Owner
        /// </summary>
        [JsonPropertyName("owner")]
        public string Owner { get; set; }

        /// <summary>
        /// Properties
        /// </summary>
        [JsonPropertyName("properties")]
        public object Properties { get; set; }

        /// <summary>
        /// Cover image
        /// </summary>
		[JsonPropertyName("collection_cover")]
        public string CollectionCover { get; set; }

		/// <summary>
		/// Implicitely convert unified Collection to UniqueCollectionRest type
		/// </summary>
		/// <param name="p"></param>
        public static implicit operator UniqueCollectionRest(Collection p) => new UniqueCollectionRest
        {
            Name = p.Name,
            Description = p.Description,

            // Owner
            Address = p.Owner,

            CoverImage = new Image
            {
                Url = p.CollectionCover,
            },
        };
    }
}

