using Substrate.NetApi.Model.Rpc;
using Substrate.NetApi.Model.Types;
using Substrate.NetApiExt.Generated;
using Substrate.NetApiExt.Generated.Model.sp_core.crypto;

namespace UniqueSDK
{
	public static class BalancesModel
	{
        /// <summary>
        /// Queries System.Account AccountInfo. Gives you the abstracted version.
        /// </summary>
        /// <param name="substrateClient"></param>
        /// <param name="accountId32"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Abstracted AccountInfo</returns>
        public static async Task<AccountInfo> GetAccountInfoAsync(
            this SubstrateClientExt substrateClient,
            AccountId32 accountId32,
            CancellationToken cancellationToken = default
        )
        {
            Substrate.NetApiExt.Generated.Model.frame_system.AccountInfo accountInfo = await substrateClient.SystemStorage.Account(
                accountId32,
                null,
                cancellationToken
            );

            return new AccountInfo(accountInfo, Constants.OPAL_DECIMALS, Constants.OPAL_UNIT);
        }

        /// <summary>
        /// Queries System.Account AccountInfo. Gives you the abstracted version.
        /// </summary>
        /// <param name="substrateClient"></param>
        /// <param name="address"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Abstracted AccountInfo</returns>
        public static async Task<AccountInfo> GetAccountInfoAsync(
            this SubstrateClientExt substrateClient,
            string address,
            CancellationToken cancellationToken = default
        )
        {
            return await substrateClient.GetAccountInfoAsync(address.ToAccountId32(), cancellationToken);
        }

        /// <summary>
        /// Queries System.Account AccountInfo. Gives you the abstracted version.
        /// </summary>
        /// <param name="substrateClient"></param>
        /// <param name="account"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Abstracted AccountInfo</returns>
        public static async Task<AccountInfo> GetAccountInfoAsync(
            this SubstrateClientExt substrateClient,
            Account account,
            CancellationToken cancellationToken = default
        )
        {
            return await substrateClient.GetAccountInfoAsync(account.ToAccountId32(), cancellationToken);
        }

        /// <summary>
        /// https://rest.unique.network/opal/swagger#/balance/transferMutation
        /// </summary>
        /// <param name="address">who</param>
        /// <param name="destinationAddress">destination</param>
        /// <param name="amount"></param>
        /// <param name="nonce"></param>
        /// <param name="use"></param>
        /// <param name="withFee"></param>
        /// <param name="verify"></param>
        /// <param name="callbackUrl"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Response</returns>
        public static async Task<Response> TransferRestAsync(
            string address,
            string destinationAddress,
            decimal amount,
            uint nonce,
            UseEnum use = UseEnum.Build,
            bool withFee = false,
            bool verify = false,
            string? callbackUrl = null,
            CancellationToken cancellationToken = default
        )
        {
            string callback = callbackUrl is null ? "" : $"&callbackUrl={callbackUrl}"; // Handle string encoding

            var url = $"{Constants.OPAL_REST_URL}/v1/balance/transfer?use={use}&withFee={withFee}&verify={verify}&nonce={nonce}{callback}";

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
        /// <param name="waitForFinality">Set to false, if you do not want to await until the Finality is confirmed</param>
        /// <param name="nonce"></param>
        /// <param name="use"></param>
        /// <param name="withFee"></param>
        /// <param name="verify"></param>
        /// <param name="callbackUrl"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Status of the submitted extrinsic</returns>
        public static async Task<ExtrinsicResult> TransferExtrinsicAsync(
            this SubstrateClientExt substrateClient,
            Account account,
            string destinationAddress,
            decimal amount,
            bool waitForFinality = true,
            Action<string, ExtrinsicStatus>? customCallback = null,
            uint? nonce = null,
            UseEnum use = UseEnum.Build,
            bool withFee = false,
            bool verify = false,
            string? callbackUrl = null,
            CancellationToken cancellationToken = default
        )
        {
            // If nonce is not provided, get a new one
            nonce ??= await substrateClient.System.AccountNextIndexAsync(account.Value, cancellationToken);

            Response response = await TransferRestAsync(
                account.Value,
                destinationAddress,
                amount,
                nonce.Value,
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
                cancellationToken
            );
        }
    }
}

