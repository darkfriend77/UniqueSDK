using Substrate.NET.Wallet.Keyring;
using Substrate.NetApi.Model.Types;
using UniqueSDK;
using Substrate.NetApi.Model.Rpc;
using Opal.NetApiExt.Generated;
using Substrate.NetApi.Model.Extrinsics;
using Newtonsoft.Json;

namespace UniqueSDKTests;

public class CollectionTests
{
    private Account account;

    private SubstrateClientExt client;

    [SetUp]
    public async Task SetupAsync()
    {
        // SdkConfig
        SdkConfig.UseDefaultNetwork = NetworkEnum.Opal;

        // Account creation
        var mnemonics = "collect salad honey track clerk energy agent empty edit devote mixed injury";

        var keyring = new Keyring();

        var firstWallet = keyring.AddFromMnemonic(mnemonics, new Meta() { Name = "Test account" }, KeyType.Ed25519);

        account = firstWallet.Account;

        // Substrate client
        client = new SubstrateClientExt(
                new System.Uri(Constants.OPAL_NODE_URL),
                ChargeTransactionPayment.Default());

        await client.ConnectAsync();
    }

    [Test]
    public async Task CreateCollectionRestTestAsync()
    {
        var collection = new UniqueCollectionRest
        {
            Address = account.Value, // Or any other string SS58 address
            CoverImage = new Image {
                Url = "https://ipfs.unique.network/ipfs/QmcAcH4F9HYQtpqKHxBFwGvkfKb8qckXj2YWUrcc8yd24G/image1.png",
            },
            Name = "Test from the C# Unique SDK",
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

        var response = await CollectionModel.CreateCollectionRestAsync(collection, 0);

        Console.WriteLine(response.SignerPayloadHex);
    }

    [Test]
    public async Task CreateCollectionTestAsync()
    {
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

        var collection = new Collection
        {
            Owner = account.Value, // Or any other string SS58 address
            CollectionCover = "https://ipfs.unique.network/ipfs/QmcAcH4F9HYQtpqKHxBFwGvkfKb8qckXj2YWUrcc8yd24G/image1.png",
            Name = "C# is BEST",
            Description = "This is the second use case of creating collection from C# Unique SDK in unit tests. Also, everyone knows that rust is better :).",
        };

        uint? collectionId = await CollectionModel.SignAndSubmitCreateCollectionExtrinsicAsync(client, account, collection, myCallback);

        Console.WriteLine("Collection id: " + collectionId);
    }

    [Test]
    public async Task CreateCollectionAndGetFeeTestAsync()
    {
        var collection = new UniqueCollectionRest
        {
            Address = account.Value, // Or any other string SS58 address
            CoverImage = new Image
            {
                Url = "https://ipfs.unique.network/ipfs/QmcAcH4F9HYQtpqKHxBFwGvkfKb8qckXj2YWUrcc8yd24G/image1.png",
            },
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

        var nonce = await client.System.AccountNextIndexAsync(account.Value, CancellationToken.None);

        var response = await CollectionModel.CreateCollectionRestAsync(collection, nonce, withFee: true);

        Console.WriteLine($"Fee: {response.Fee.Amount} {response.Fee.Unit}");

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

        uint? collectionId = await client.SignAndSubmitCreateCollectionExtrinsicAsync(account, response, myCallback);

        Console.WriteLine("Collection id: " + collectionId);

    }

    [Test]
    public async Task GetCollectionByIdAsync()
    {
        var collectionId = 2753;

        var collection = await CollectionModel.GetCollectionByIdAsync(collectionId);

        Assert.NotNull(collection);
    }

    [Test]
    public async Task GetCollectionByNameAsync()
    {
        var collectionName = "C# is BEST";

        var collections = await CollectionModel.GetCollectionsByNameAsync(collectionName, limit: 2);

        Assert.That(collections.Any());
    }

    [Test]
    public async Task GetCollectionListByOwnerAsync()
    {
        var owner = account.Value;

        var collections = await CollectionModel.GetCollectionsByOwnerAsync(owner);

        Assert.That(collections.Any());
    }
}

