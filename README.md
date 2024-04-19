# Setup
```C#
// Account creation
var mnemonics = "collect salad honey track clerk energy agent empty edit devote mixed injury";

var keyring = new Keyring();

var firstWallet = keyring.AddFromMnemonic(mnemonics, new Meta() { Name = "Test account" }, KeyType.Ed25519);

var account = firstWallet.Account;

// Substrate client
var client = new SubstrateClientExt(
        new System.Uri(Constants.OPAL_NODE_URL),
        ChargeTransactionPayment.Default());

await client.ConnectAsync();
```

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
