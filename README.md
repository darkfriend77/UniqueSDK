# Unique C# SDK
Simple to use nuget package (standard for C# packages) that abstracts the use of simple NFT operations (minting, transferring, querying..).

The ideal goal of this abstraction is to make it understandable to non-web3 developers.

# Create a new account
```C#
// Generate new mnemonics
var mnemonics = Mnemonic.GenerateMnemonic(MnemonicSize.Words12);

// Show these mnemonics to the user.
// Careful, mnemonics are private data, which should never be leaked.
Console.WriteLine(mnemonics);

var keyring = new Keyring();

var firstWallet = keyring.AddFromMnemonic(mnemonics, new Meta() { Name = "Test account" }, KeyType.Ed25519);

// Your account that you can use
var account = firstWallet.Account;
```

# Use existing account
```C#
// Use existing mnemonics
var mnemonics = "collect salad honey track clerk energy agent empty edit devote mixed injury";

var keyring = new Keyring();

var firstWallet = keyring.AddFromMnemonic(mnemonics, new Meta() { Name = "Test account" }, KeyType.Ed25519);

// Your account that you can use
var account = firstWallet.Account;
```

# Connect to a Substrate node
```C#
// Connect to a Substrate node
var client = new SubstrateClientExt(
        new System.Uri(Constants.UNIQUE_NODE_URL),
        ChargeTransactionPayment.Default());

await client.ConnectAsync();
```

- Feel free to change the `Constants.UNIQUE_NODE_URL` for any other websocket node URL.

# Query Free Balance Example
```C#
AccountInfo accountInfo = await client.GetAccountInfoAsync(account);

Console.WriteLine($"Free balance: {accountInfo.Data.Free.Amount} {accountInfo.Data.Free.Unit}");
```

```C#
string destinationAddress = "5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y";

AccountInfo accountInfo = await client.GetAccountInfoAsync(destinationAddress);

Console.WriteLine($"Free balance: {accountInfo.Data.Free.Amount} {accountInfo.Data.Free.Unit}");
```

# Balance Transfer Example

```C#
var nonce = await client.System.AccountNextIndexAsync(account.Value, CancellationToken.None);

Response response = await BalancesModel.TransferRestAsync(
account.Value,
"5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y",
(decimal)1.5,
nonce,
withFee: true
);

Console.WriteLine($"Fee: {response.Fee.Amount} {response.Fee.Unit}");

var result = await client.SignAndSubmitExtrinsicAsync(await response.SignerPayloadJSON.ToExtrinsicAsync(account));

Assert.That(result == ExtrinsicResult.Success);
```

# Create Collection Example

```c#
Action<string, ExtrinsicStatus> myCallback = (string id, ExtrinsicStatus status) =>
{
    if (status.ExtrinsicState == ExtrinsicState.Ready)
    {
        Console.WriteLine("Ready");
    }
    else if (status.ExtrinsicState == ExtrinsicState.Dropped)
    {
        Console.WriteLine("Dropped");
    }
    else if (status.ExtrinsicState == ExtrinsicState.InBlock)
    {
        Console.WriteLine("In block");
    }
    else if (status.ExtrinsicState == ExtrinsicState.Finalized)
    {
        Console.WriteLine("Finalized");
    }
};

Collection collection = new Collection
{
    Address = account.Value, // Or any other string SS58 address
    CoverImage = new Image
    {
        Url = "https://ipfs.unique.network/ipfs/QmcAcH4F9HYQtpqKHxBFwGvkfKb8qckXj2YWUrcc8yd24G/image1.png",
    },
    Name = "Test Collection 5",
    PotentialAttributes = new List<PotentialAttribute> {
        new PotentialAttribute{
            TraitType = "color",
            Values = new List<string>
            {
                "red",
                "green",
                "blue"
            }
        }
    }
};

uint? collectionId = await CollectionModel.SignAndSubmitCreateCollectionExtrinsicAsync(client, account, collection, myCallback);

Console.WriteLine("Collection id: " + collectionId);
```

# Mint NFT (Full example)

```C#
// Use existing mnemonics
var mnemonics = "collect salad honey track clerk energy agent empty edit devote mixed injury";

var keyring = new Keyring();

var firstWallet = keyring.AddFromMnemonic(mnemonics, new Meta() { Name = "Test account" }, KeyType.Ed25519);

// Your account that you can use
var account = firstWallet.Account;

// Connect to a Substrate node
var client = new SubstrateClientExt(
        new System.Uri(Constants.UNIQUE_NODE_URL),
        ChargeTransactionPayment.Default());

await client.ConnectAsync();

// Nft data
var nft = new Nft
{
    CollectionId = 2753,
    Owner = account.Value, // Or any other string SS58 address
    TokenName = "Unified 2",
    ImageSource = "https://bafybeie5r4xjzjn3x6tl7anncjg62yhjzirpzucve4bwpfjhmblzsfpsuy.ipfs.nftstorage.link/",
};

// Custom callback to react on the extrinsic status change
Action<string, ExtrinsicStatus> myCallback = (string id, ExtrinsicStatus status) =>
{
    if (status.ExtrinsicState == ExtrinsicState.Ready)
    {
        Console.WriteLine("Ready");
    }
    else if (status.ExtrinsicState == ExtrinsicState.Dropped)
    {
        Console.WriteLine("Dropped");
    }
    else if (status.ExtrinsicState == ExtrinsicState.InBlock)
    {
        Console.WriteLine("In block");
    }
    else if (status.ExtrinsicState == ExtrinsicState.Finalized)
    {
        Console.WriteLine("Finalized");
    }
};

// Sign and Submit the Mint Nft extrinsic
uint? nftId = await client.SignAndSubmitMintNftExtrinsicAsync(account, nft, myCallback);

// Your newly minted Nft id
Console.WriteLine("Nft id: " + nftId);
```

# Mint NFT and get Fee details (Full example)

```C#
// Use existing mnemonics
var mnemonics = "collect salad honey track clerk energy agent empty edit devote mixed injury";

var keyring = new Keyring();

var firstWallet = keyring.AddFromMnemonic(mnemonics, new Meta() { Name = "Test account" }, KeyType.Ed25519);

// Your account that you can use
var account = firstWallet.Account;

// Connect to a Substrate node
var client = new SubstrateClientExt(
        new System.Uri(Constants.UNIQUE_NODE_URL),
        ChargeTransactionPayment.Default());

await client.ConnectAsync();

// Get Account nonce
var nonce = await client.System.AccountNextIndexAsync(account.Value, CancellationToken.None);

// Nft data
var nft = new Nft
{
    CollectionId = 2753,
    Owner = account.Value, // Or any other string SS58 address
    TokenName = "Unified",
    ImageSource = "https://bafybeie5r4xjzjn3x6tl7anncjg62yhjzirpzucve4bwpfjhmblzsfpsuy.ipfs.nftstorage.link/",
};

// Get extrinsic details from REST api
var response = await NftModel.MintNftRestAsync(
    nft,
    nonce,
    withFee: true
);

// Show the Fee to the user
Console.WriteLine($"Fee: {response.Fee.Amount} {response.Fee.Unit}");

// Custom callback to react on the extrinsic status change
Action<string, ExtrinsicStatus> myCallback = (string id, ExtrinsicStatus status) =>
{
    if (status.ExtrinsicState == ExtrinsicState.Ready)
    {
        Console.WriteLine("Ready");
    }
    else if (status.ExtrinsicState == ExtrinsicState.Dropped)
    {
        Console.WriteLine("Dropped");
    }
    else if (status.ExtrinsicState == ExtrinsicState.InBlock)
    {
        Console.WriteLine("In block");
    }
    else if (status.ExtrinsicState == ExtrinsicState.Finalized)
    {
        Console.WriteLine("Finalized");
    }
};

// Sign and Submit the Mint Nft extrinsic
uint? nftId = await client.SignAndSubmitMintNftExtrinsicAsync(account, response, myCallback);

// Your newly minted Nft id
Console.WriteLine("Nft id: " + nftId);
```

# Query Collection data
```C#
var collectionId = 2753;

var collection = await CollectionModel.GetCollectionByIdAsync(collectionId);
```

```C#
var collectionName = "C# is BEST";

var collections = await CollectionModel.GetCollectionsByNameAsync(collectionName, limit: 2);
```

```C#
var owner = account.Value;

var collections = await CollectionModel.GetCollectionsByOwnerAsync(owner);
```
