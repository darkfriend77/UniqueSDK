using System.Text.Json.Serialization;

namespace UniqueSDK
{
    public class UniqueCollectionRest
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

        [JsonPropertyName("tokenPrefix")]
        public string TokenPrefix { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("cover_image")]
        public Image CoverImage { get; set; }

        [JsonPropertyName("default_token_image")]
        public Image DefaultTokenImage { get; set; }

        [JsonPropertyName("customizing")]
        public Customizing Customizing { get; set; }

        [JsonPropertyName("royalties")]
        public List<Royalty> Royalties { get; set; }

        [JsonPropertyName("potential_attributes")]
        public List<PotentialAttribute> PotentialAttributes { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("encodeOptions")]
        public EncodeOptions EncodeOptions { get; set; }

        [JsonPropertyName("mode")]
        public string Mode { get; set; }

        [JsonPropertyName("sponsorship")]
        public Sponsorship Sponsorship { get; set; }

        [JsonPropertyName("limits")]
        public Limits Limits { get; set; }

        [JsonPropertyName("metaUpdatePermission")]
        public string MetaUpdatePermission { get; set; }

        [JsonPropertyName("permissions")]
        public Permissions Permissions { get; set; }

        [JsonPropertyName("readOnly")]
        public bool ReadOnly { get; set; }
    }

    public class Image
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("details")]
        public ImageDetails Details { get; set; }

        [JsonPropertyName("thumbnail")]
        public Thumbnail Thumbnail { get; set; }
    }

    public class ImageDetails
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("bytes")]
        public int Bytes { get; set; }

        [JsonPropertyName("format")]
        public string Format { get; set; }

        [JsonPropertyName("sha256")]
        public string Sha256 { get; set; }

        [JsonPropertyName("width")]
        public int Width { get; set; }

        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonPropertyName("order")]
        public int Order { get; set; }

        [JsonPropertyName("duration")]
        public int Duration { get; set; }

        [JsonPropertyName("codecs")]
        public List<string> Codecs { get; set; }

        [JsonPropertyName("loop")]
        public bool Loop { get; set; }
    }

    public class Thumbnail : ImageDetails
    {

    }

    public class Customizing
    {
        [JsonPropertyName("self")]
        public Self Self { get; set; }

        [JsonPropertyName("slots")]
        public List<Slot> Slots { get; set; }

        [JsonPropertyName("mutators")]
        public List<string> Mutators { get; set; }

        [JsonPropertyName("mutator_reactions")]
        public List<MutatorReaction> MutatorReactions { get; set; }
    }

    public class Self
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("details")]
        public ImageDetails Details { get; set; }

        [JsonPropertyName("image_overlay_specs")]
        public ImageOverlaySpecs ImageOverlaySpecs { get; set; }

        [JsonPropertyName("placeholder")]
        public Placeholder Placeholder { get; set; }

        [JsonPropertyName("tag")]
        public string Tag { get; set; }
    }

    public class Slot
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("collections")]
        public List<UniqueCollectionRest> Collections { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("image_overlay_specs")]
        public ImageOverlaySpecs ImageOverlaySpecs { get; set; }
    }

    public class MutatorReaction : ImageDetails
    {

    }

    public class Royalty
    {
        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("percent")]
        public int Percent { get; set; }

        [JsonPropertyName("isPrimaryOnly")]
        public bool IsPrimaryOnly { get; set; }
    }

    public class PotentialAttribute
    {
        [JsonPropertyName("trait_type")]
        public string TraitType { get; set; }

        [JsonPropertyName("display_type")]
        public string DisplayType { get; set; }

        [JsonPropertyName("values")]
        public List<string> Values { get; set; }
    }

    public class EncodeOptions
    {
        [JsonPropertyName("defaultPermission")]
        public DefaultPermission DefaultPermission { get; set; }

        [JsonPropertyName("overwriteProperties")]
        public List<PropertyOverride> OverwriteProperties { get; set; }

        [JsonPropertyName("overwriteTPPs")]
        public List<TPPOverride> OverwriteTPPs { get; set; }
    }

    public class DefaultPermission
    {
        [JsonPropertyName("mutable")]
        public bool Mutable { get; set; }

        [JsonPropertyName("collectionAdmin")]
        public bool CollectionAdmin { get; set; }

        [JsonPropertyName("tokenOwner")]
        public bool TokenOwner { get; set; }
    }

    public class PropertyOverride
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class TPPOverride
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("permission")]
        public DefaultPermission Permission { get; set; }
    }

    public class Sponsorship
    {
        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("isConfirmed")]
        public bool IsConfirmed { get; set; }
    }

    public class Limits
    {
        [JsonPropertyName("accountTokenOwnershipLimit")]
        public int AccountTokenOwnershipLimit { get; set; }

        [JsonPropertyName("sponsoredDataSize")]
        public int SponsoredDataSize { get; set; }

        [JsonPropertyName("sponsoredDataRateLimit")]
        public int SponsoredDataRateLimit { get; set; }

        [JsonPropertyName("tokenLimit")]
        public int TokenLimit { get; set; }

        [JsonPropertyName("sponsorTransferTimeout")]
        public int SponsorTransferTimeout { get; set; }

        [JsonPropertyName("sponsorApproveTimeout")]
        public int SponsorApproveTimeout { get; set; }

        [JsonPropertyName("ownerCanTransfer")]
        public bool OwnerCanTransfer { get; set; }

        [JsonPropertyName("ownerCanDestroy")]
        public bool OwnerCanDestroy { get; set; }

        [JsonPropertyName("transfersEnabled")]
        public bool TransfersEnabled { get; set; }
    }

    public class Permissions
    {
        [JsonPropertyName("access")]
        public string Access { get; set; }

        [JsonPropertyName("mintMode")]
        public bool MintMode { get; set; }

        [JsonPropertyName("nesting")]
        public Nesting Nesting { get; set; }
    }

    public class Nesting
    {
        [JsonPropertyName("tokenOwner")]
        public bool TokenOwner { get; set; }

        [JsonPropertyName("collectionAdmin")]
        public bool CollectionAdmin { get; set; }

        [JsonPropertyName("restricted")]
        public List<int> Restricted { get; set; }
    }

    public class ImageOverlaySpecs
    {
        [JsonPropertyName("layer")]
        public int Layer { get; set; }

        [JsonPropertyName("order_in_layer")]
        public int OrderInLayer { get; set; }

        [JsonPropertyName("offset")]
        public Offset Offset { get; set; }

        [JsonPropertyName("opacity")]
        public float Opacity { get; set; }

        [JsonPropertyName("rotation")]
        public float Rotation { get; set; }

        [JsonPropertyName("scale")]
        public Scale Scale { get; set; }

        [JsonPropertyName("mount_point")]
        public MountPoint MountPoint { get; set; }

        [JsonPropertyName("parent_mount_point")]
        public MountPoint ParentMountPoint { get; set; }
    }

    public class Offset
    {
        [JsonPropertyName("x")]
        public int X { get; set; }

        [JsonPropertyName("y")]
        public int Y { get; set; }
    }

    public class Scale
    {
        [JsonPropertyName("x")]
        public int X { get; set; }

        [JsonPropertyName("y")]
        public int Y { get; set; }

        [JsonPropertyName("unit")]
        public string Unit { get; set; }
    }

    public class MountPoint
    {
        [JsonPropertyName("x")]
        public int X { get; set; }

        [JsonPropertyName("y")]
        public int Y { get; set; }
    }

    public class Placeholder : ImageDetails
    {

    }
}

