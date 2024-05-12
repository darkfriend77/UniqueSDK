using System.Text.Json.Serialization;

namespace UniqueSDK
{
    public class UniqueNftRest
    {
        [JsonPropertyName("schemaName")]
        public string SchemaName { get; set; }

        [JsonPropertyName("schemaVersion")]
        public string SchemaVersion { get; set; }

        [JsonPropertyName("originalSchemaVersion")]
        public string OriginalSchemaVersion { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("image")]
        public string Image { get; set; }

        [JsonPropertyName("image_details")]
        public ImageDetails ImageDetails { get; set; }

        [JsonPropertyName("media")]
        public List<Media> Media { get; set; }

        [JsonPropertyName("animation_url")]
        public string AnimationUrl { get; set; }

        [JsonPropertyName("animation_details")]
        public ImageDetails AnimationDetails { get; set; }

        [JsonPropertyName("youtube_url")]
        public string YoutubeUrl { get; set; }

        [JsonPropertyName("created_by")]
        public string CreatedBy { get; set; }

        [JsonPropertyName("external_url")]
        public string ExternalUrl { get; set; }

        [JsonPropertyName("customizing")]
        public Customizing Customizing { get; set; }

        [JsonPropertyName("customizing_overrides")]
        public Customizing CustomizingOverrides { get; set; }

        [JsonPropertyName("background_color")]
        public string BackgroundColor { get; set; }

        [JsonPropertyName("attributes")]
        public List<NftAttribute> Attributes { get; set; }

        [JsonPropertyName("royalties")]
        public List<Royalty> Royalties { get; set; }

        [JsonPropertyName("locale")]
        public string Locale { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("owner")]
        public string Owner { get; set; }

        [JsonPropertyName("collectionId")]
        public int CollectionId { get; set; }

        [JsonPropertyName("encodeOptions")]
        public EncodeOptions EncodeOptions { get; set; }
    }

    public class NftAttribute
    {
        [JsonPropertyName("trait_type")]
        public string TraitType { get; set; }

        [JsonPropertyName("display_type")]
        public string DisplayType { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class Media
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("details")]
        ImageDetails Details { get; set; }

        [JsonPropertyName("thumbnail")]
        public Thumbnail Thumbnail { get; set; }

        [JsonPropertyName("poster")]
        public Poster Poster { get; set; }
    }

    public class Poster : Media { }
}