using Newtonsoft.Json;
using Substrate.NET.Wallet.Keyring;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Types;
using Substrate.Opal.NET.NetApiExt.Generated;
using UniqueSDK;

namespace UniqueSDKTests
{
	public class BalancesTest
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
                    new System.Uri(UniqueSDK.Constants.OPAL_NODE_URL),
                    ChargeTransactionPayment.Default());

            await client.ConnectAsync();
        }

        [Test]
        public async Task GetAccountInfoFromAccountTestAsync()
        {
            var accountInfo = await client.GetAccountInfoAsync(account);

            Assert.That(accountInfo.Nonce > 0);

            Console.WriteLine($"Free balance: {accountInfo.Data.Free.Amount} {accountInfo.Data.Free.Unit}");
        }

        [Test]
        public async Task GetAccountInfoFromAddressTestAsync()
        {
            var accountInfo = await client.GetAccountInfoAsync("5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y");

            Assert.That(accountInfo.Nonce > 0);

            Console.WriteLine($"Free balance: {accountInfo.Data.Free.Amount} {accountInfo.Data.Free.Unit}");
        }

        [Test]
        public async Task TransferRestTestAsync()
        {
            var response = await BalancesModel.TransferRestAsync(
                account.Value,
                "5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y",
                (decimal)1.5,
                0);

            Console.WriteLine(JsonConvert.SerializeObject(response));
        }

        [Test]
        public async Task TransferSuccessAndGetFeeTestAsync()
        {
            var nonce = await client.System.AccountNextIndexAsync(account.Value, CancellationToken.None);

            var response = await BalancesModel.TransferRestAsync(
                account.Value,
                "5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y",
                (decimal)1.5,
                nonce,
                withFee: true
            );

            Console.WriteLine($"Fee: {response.Fee.Amount} {response.Fee.Unit}");

            var result = await client.SignAndSubmitExtrinsicAsync(await response.SignerPayloadJSON.ToExtrinsicAsync(account));

            Assert.That(result == ExtrinsicResult.Success);
        }

        [Test]
        public async Task TransferFailedTestAsync()
        {
            var nonce = await client.System.AccountNextIndexAsync(account.Value, CancellationToken.None);

            var response = await BalancesModel.TransferRestAsync(
                account.Value,
                "5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y",
                10000000,
                nonce
            );

            var result = await client.SignAndSubmitExtrinsicAsync(await response.SignerPayloadJSON.ToExtrinsicAsync(account));

            Assert.That(result == ExtrinsicResult.Failed);
        }

        [Test]
        public async Task TransferSubmittedTestAsync()
        {
            // Can be also used alternativelly like this :)
            var result = await BalancesModel.SignAndSubmitTransferExtrinsicAsync(
                client,
                account,
                "5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y",
                (decimal)1.5,
                waitForFinality: false,
                withFee: true
            );

            Assert.That(result == ExtrinsicResult.Submitted);
        }
    }
}

