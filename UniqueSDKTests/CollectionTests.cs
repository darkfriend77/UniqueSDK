using Substrate.NET.Wallet.Keyring;
using Substrate.NetApi.Model.Types;
using UniqueSDK;
using Substrate.NetApi.Model.Rpc;
using Substrate.NetApiExt.Generated;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi;

namespace UniqueSDKTests;

public class CollectionTests
{
    private Account account;

    private SubstrateClientExt client;

    [SetUp]
    public async Task SetupAsync()
    {
        // Account creation
        var mnemonics = "collect salad honey track clerk energy agent empty edit devote mixed injury";

        var keyring = new Keyring();

        var firstWallet = keyring.AddFromMnemonic(mnemonics, new Meta() { Name = "Test account" }, KeyType.Ed25519);

        account = firstWallet.Account;

        // Substrate client
        client = new SubstrateClientExt(
                new System.Uri(UniqueSDK.Constants.OPAL_NODE_URL),
                ChargeTransactionPayment.Default());

        await client.ConnectAsync();
    }

    [Test]
    public async Task CreateCollectionRestTestAsync()
    {
        Collection collection = new Collection
        {
            Address = account.Value, // Or any other string SS58 address
            CoverImage = new Image {
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

        Response response = await CollectionModel.CreateCollectionRestAsync(collection, 0);

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
    }

    [Test]
    public async Task CreateCollectionAndGetFeeTestAsync()
    {
        Collection collection = new Collection
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

        Response response = await CollectionModel.CreateCollectionRestAsync(collection, nonce, withFee: true);

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

        uint? collectionId = await CollectionModel.SignAndSubmitCreateCollectionExtrinsicAsync(client, account, response, myCallback);

        Console.WriteLine("Collection id: " + collectionId);
    }
}
