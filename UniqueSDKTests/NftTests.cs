using Substrate.NET.Wallet.Keyring;
using Substrate.NetApi.Model.Types;
using UniqueSDK;
using Substrate.NetApi.Model.Rpc;
using Substrate.NetApiExt.Generated;
using Substrate.NetApi.Model.Extrinsics;
using Newtonsoft.Json;

namespace UniqueSDKTests;

public class NftTests
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
    public async Task MintNftRestTestAsync()
    {
        var nft = new UniqueNftRest
        {
            CollectionId = 2753,
            Owner = account.Value, // Or any other string SS58 address
            Address = account.Value, // Or any other string SS58 address
            Name = "My first NFT from C# Unique SDK",
            Description = "Very excited!",
            Image = "https://bafybeie5r4xjzjn3x6tl7anncjg62yhjzirpzucve4bwpfjhmblzsfpsuy.ipfs.nftstorage.link/",
            ImageDetails = new()
            {
                Format = "jpeg",
                Width = 1920,
                Height = 1438,
                Bytes = 788523,
                Type = "image",
                Sha256 = "f82f07e8dd56abdfd68db80bef76f306675e04d336d795f86415ccf1c1d722fa",
            },
            Attributes = new List<NftAttribute> {
                new NftAttribute{
                    TraitType = "color",
                    Value = "orange"
                },
                new NftAttribute{
                    TraitType = "shape",
                    Value = "mountains"
                }
            }
        };

        var response = await NftModel.MintNftRestAsync(nft, 0);

        Console.WriteLine(response.SignerPayloadRaw.Data);
    }

    [Test]
    public async Task SignAndSubmitMintNftWithFeeTestAsync()
    {
        var nft = new Nft
        {
            CollectionId = 2753,
            Owner = account.Value, // Or any other string SS58 address
            TokenName = "Unified",
            ImageSource = "https://bafybeie5r4xjzjn3x6tl7anncjg62yhjzirpzucve4bwpfjhmblzsfpsuy.ipfs.nftstorage.link/",
        };

        var nonce = await client.System.AccountNextIndexAsync(account.Value, CancellationToken.None);

        var response = await NftModel.MintNftRestAsync(
            nft,
            nonce,
            withFee: true
        );

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

        uint? nftId = await client.SignAndSubmitMintNftExtrinsicAsync(account, response, myCallback);

        Console.WriteLine("Nft id: " + nftId);
    }

    [Test]
    public async Task GetNftsByIdAsync()
    {
        var collectionId = 2753;

        var nftId = 2;

        var nft = await NftModel.GetNftByIdAsync(collectionId, nftId);

        Assert.NotNull(nft);

        Console.WriteLine(JsonConvert.SerializeObject(nft));
    }

    [Test]
    public async Task GetNftsByCollectionIdAsync()
    {
        var collectionId = 2753;

        var nfts = await NftModel.GetNftListByCollectionIdAsync(collectionId);

        Assert.That(nfts.Count() >= 3);
    }

    [Test]
    public async Task GetNftsByCollectionNameAsync()
    {
        var name = "Ancient Opals";

        var nfts = await NftModel.GetNftListByCollectionNameAsync(name);

        Assert.That(nfts.Any());
    }

    [Test]
    public async Task GetNftsByOwnerAsync()
    {
        var owner = account.Value;

        var nfts = await NftModel.GetNftListByOwnerAsync(owner);

        Assert.That(nfts.Any());
    }
}

