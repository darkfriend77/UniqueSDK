using Substrate.NetApi.Model.Rpc;
using Substrate.NetApi.Model.Types;
using Opal.NetApiExt.Generated;
using Opal.NetApiExt.Generated.Model.sp_core.crypto;

namespace UniqueSDK
{
	public static class BalancesModel
	{
        /// <summary>
        /// Queries System.Account AccountInfo. Gives you the abstracted version.
        /// </summary>
        /// <param name="substrateClient"></param>
        /// <param name="accountId32"></param>
        /// <param name="network">Network you want to use</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Abstracted AccountInfo</returns>
        public static async Task<AccountInfo> GetAccountInfoAsync(
            this SubstrateClientExt substrateClient,
            AccountId32 accountId32,
            NetworkEnum? network = null,
            CancellationToken cancellationToken = default
        )
        {
            // If network is not provided, use the default one
            network ??= SdkConfig.UseDefaultNetwork;

            Opal.NetApiExt.Generated.Model.frame_system.AccountInfo accountInfo = await substrateClient.SystemStorage.Account(
                accountId32,
                null,
                cancellationToken
            );

            return new AccountInfo(accountInfo, Constants.GetDecimals(network), Constants.GetUnit(network));
        }

        /// <summary>
        /// Queries System.Account AccountInfo. Gives you the abstracted version.
        /// </summary>
        /// <param name="substrateClient"></param>
        /// <param name="address"></param>
        /// <param name="network">Network you want to use</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Abstracted AccountInfo</returns>
        public static async Task<AccountInfo> GetAccountInfoAsync(
            this SubstrateClientExt substrateClient,
            string address,
            NetworkEnum? network = null,
            CancellationToken cancellationToken = default
        )
        {
            return await substrateClient.GetAccountInfoAsync(address.ToAccountId32(), network, cancellationToken);
        }

        /// <summary>
        /// Queries System.Account AccountInfo. Gives you the abstracted version.
        /// </summary>
        /// <param name="substrateClient"></param>
        /// <param name="account"></param>
        /// <param name="network">Network you want to use</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Abstracted AccountInfo</returns>
        public static async Task<AccountInfo> GetAccountInfoAsync(
            this SubstrateClientExt substrateClient,
            Account account,
            NetworkEnum? network = null,
            CancellationToken cancellationToken = default
        )
        {
            return await substrateClient.GetAccountInfoAsync(account.ToAccountId32(), network, cancellationToken);
        }

        /// <summary>
        /// https://rest.unique.network/opal/swagger#/balance/transferMutation
        /// </summary>
        /// <param name="address">who</param>
        /// <param name="destinationAddress">destination</param>
        /// <param name="amount"></param>
        /// <param name="nonce"></param>
        /// <param name="network">Network you want to use</param>
        /// <param name="use"></param>
        /// <param name="withFee"></param>
        /// <param name="verify"></param>
        /// <param name="callbackUrl"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Response</returns>
        public static async Task<RestResponse> TransferRestAsync(
            string address,
            string destinationAddress,
            decimal amount,
            uint nonce,
            NetworkEnum? network = null,
            UseEnum use = UseEnum.Build,
            bool withFee = false,
            bool verify = false,
            string? callbackUrl = null,
            CancellationToken cancellationToken = default
        )
        {
            // If network is not provided, use the default one
            network ??= SdkConfig.UseDefaultNetwork;

            string callback = callbackUrl is null ? "" : $"&callbackUrl={callbackUrl}"; // Handle string encoding

            var url = $"{Constants.GetRestUrl(network)}/v1/balance/transfer?use={use}&withFee={withFee}&verify={verify}&nonce={nonce}{callback}";

            var transfer = new Transfer
            {
                Address = address,
                Destination = destinationAddress,
                Amount = amount,
            };

            return await RestModel.ExecutePostAsync(url, transfer, cancellationToken);
        }

        /// <summary>
        /// Constructs the Balances.transfer extrinsic, signs it, submits it to the chain,
        /// and listens to on-chain events.
        /// </summary>
        /// <param name="substrateClient"></param>
        /// <param name="account"></param>
        /// <param name="destinationAddress"></param>
        /// <param name="amount"></param>
        /// <param name="customCallback"></param>
        /// <param name="network">Network you want to use</param>
        /// <param name="waitForFinality">Set to false, if you do not want to await until the Finality is confirmed</param>
        /// <param name="nonce"></param>
        /// <param name="use"></param>
        /// <param name="withFee"></param>
        /// <param name="verify"></param>
        /// <param name="callbackUrl"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="finalityTimeout">The maximum amount of time to wait for the finality. Defaultly wait maximum 60 seconds.</param>
        /// <returns>Status of the submitted extrinsic</returns>
        public static async Task<ExtrinsicResult> SignAndSubmitTransferExtrinsicAsync(
            this SubstrateClientExt substrateClient,
            Account account,
            string destinationAddress,
            decimal amount,
            bool waitForFinality = true,
            NetworkEnum? network = null,
            Action<string, ExtrinsicStatus>? customCallback = null,
            uint? nonce = null,
            UseEnum use = UseEnum.Build,
            bool withFee = false,
            bool verify = false,
            string? callbackUrl = null,
            int finalityTimeout = 60_000,
            CancellationToken cancellationToken = default
        )
        {
            // If network is not provided, use the default one
            network ??= SdkConfig.UseDefaultNetwork;

            // If nonce is not provided, get a new one
            nonce ??= await substrateClient.System.AccountNextIndexAsync(account.Value, cancellationToken);

            var response = await TransferRestAsync(
                account.Value,
                destinationAddress,
                amount,
                nonce.Value,
                network,
                use,
                withFee,
                verify,
                callbackUrl,
                cancellationToken
            );

            return await substrateClient.SignAndSubmitExtrinsicAsync(
                await response.SignerPayloadJSON.ToExtrinsicAsync(account),
                waitForFinality,
                customCallback,
                finalityTimeout,
                cancellationToken
            );
        }
    }
}

