using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Rpc;
using Substrate.NetApi.Model.Types;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;
using Substrate.Opal.NET.NetApiExt.Generated;
using Substrate.Opal.NET.NetApiExt.Generated.Model.frame_system;
using Substrate.Opal.NET.NetApiExt.Generated.Model.up_data_structs;

namespace UniqueSDK
{
	public static class NftModel
	{
        /// <summary>
        /// https://rest.unique.network/opal/swagger#/tokens/createNewTokenV2Mutation
        /// </summary>
        /// <param name="nft">Nft data</param>
        /// <param name="nonce">Nonce of the account</param>
        /// <param name="network">Network you want to use</param>
        /// <param name="use"></param>
        /// <param name="withFee"></param>
        /// <param name="verify"></param>
        /// <param name="callbackUrl"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<RestResponse> MintNftRestAsync(
            UniqueNftRest nft,
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

            var url = $"{Constants.GetRestUrl(network)}/v1/tokens/v2?use={use}&withFee={withFee}&verify={verify}&nonce={nonce}{callback}";

            return await RestModel.ExecutePostAsync(url, nft, cancellationToken);
        }

        /// <summary>
        /// Constructs the Unique.createItem extrinsic, signs it, submits it to the chain,
        /// listens to on-chain events and filters the events for Common.ItemCreated
        /// to get the TokenId of the newly created Token.
        /// </summary>
        /// <param name="substrateClient"></param>
        /// <param name="account"></param>
        /// <param name="nft">Nft data</param>
        /// <param name="customCallback"></param>
        /// <param name="nonce"></param>
        /// <param name="network">Network you want to use</param>
        /// <param name="use"></param>
        /// <param name="withFee"></param>
        /// <param name="verify"></param>
        /// <param name="callbackUrl"></param>
        /// <param name="signed"></param>
        /// <param name="finalityTimeout">The maximum amount of time to wait for the finality. Defaultly wait maximum 60 seconds.</param> 
        /// <param name="cancellationToken"></param>
        /// <returns>Token Id of the newly created Token</returns>
        public static async Task<uint?> SignAndSubmitMintNftExtrinsicAsync(
            this SubstrateClientExt substrateClient,
            Account account,
            UniqueNftRest nft,
            Action<string, ExtrinsicStatus>? customCallback = null,
            uint? nonce = null,
            NetworkEnum? network = null,
            UseEnum use = UseEnum.Build,
            bool withFee = false,
            bool verify = false,
            string? callbackUrl = null,
            bool signed = true,
            int finalityTimeout = 60_000,
            CancellationToken cancellationToken = default
        )
        {
            // If network is not provided, use the default one
            network ??= SdkConfig.UseDefaultNetwork;

            // If nonce is not provided, get a new one
            nonce ??= await substrateClient.System.AccountNextIndexAsync(account.Value, cancellationToken);

            var response = await NftModel.MintNftRestAsync(
                nft,
                nonce.Value,
                network,
                use,
                withFee,
                verify,
                callbackUrl,
                cancellationToken
            );

            return await substrateClient.SignAndSubmitMintNftExtrinsicAsync(
                account,
                response,
                customCallback,
                signed,
                finalityTimeout,
                cancellationToken
            );
        }

        /// <summary>
        /// Constructs the Unique.createItem extrinsic, signs it, submits it to the chain,
        /// listens to on-chain events and filters the events for Common.ItemCreated
        /// to get the TokenId of the newly created Token.
        /// </summary>
        /// <param name="substrateClient"></param>
        /// <param name="account"></param>
        /// <param name="response"></param>
        /// <param name="customCallback"></param>
        /// <param name="signed"></param>
        /// <param name="finalityTimeout">The maximum amount of time to wait for the finality. Defaultly wait maximum 60 seconds.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Token Id of the newly created Token</returns>
        public static async Task<uint?> SignAndSubmitMintNftExtrinsicAsync(
            this SubstrateClientExt substrateClient,
            Account account,
            RestResponse response,
            Action<string, ExtrinsicStatus>? customCallback = null,
            bool signed = true,
            int finalityTimeout = 60_000,
            CancellationToken cancellationToken = default
        )
        {
            UnCheckedExtrinsic unCheckedExtrinsic = await response.SignerPayloadJSON.ToExtrinsicAsync(account, signed);

            var tokenIdTask = new TaskCompletionSource<uint?>();

#pragma warning disable VSTHRD101 // Avoid unsupported async delegates
            Action<string, ExtrinsicStatus> callback = async (string id, ExtrinsicStatus status) =>
            {
                customCallback?.Invoke(id, status);

                switch (status.ExtrinsicState)
                {
                    case ExtrinsicState.Retracted:
                    case ExtrinsicState.FinalityTimeout:
                    case ExtrinsicState.Dropped:
                    case ExtrinsicState.Invalid:
                    case ExtrinsicState.Usurped:
                        tokenIdTask.TrySetResult(null);
                        break;

                    case ExtrinsicState.Finalized:

                        IEnumerable<EventRecord> allExtrinsicEvents;

                        try
                        {
                            allExtrinsicEvents = await EventsModel.GetExtrinsicEventsAsync(substrateClient, status.Hash, unCheckedExtrinsic);
                        }
                        catch
                        {
                            tokenIdTask.TrySetResult(null);

                            return;
                        }

                        foreach (var e in allExtrinsicEvents)
                        {
                            // Filter only Common.CollectionCreated events
                            if (e.Event.Value == Substrate.Opal.NET.NetApiExt.Generated.Model.opal_runtime.RuntimeEvent.Common)
                            {
                                var commonEvent = (Substrate.Opal.NET.NetApiExt.Generated.Model.pallet_common.pallet.EnumEvent)e.Event.Value2;

                                if (commonEvent.Value == Substrate.Opal.NET.NetApiExt.Generated.Model.pallet_common.pallet.Event.ItemCreated)
                                {
                                    var createdItemEvent = (BaseTuple<CollectionId, TokenId, Substrate.Opal.NET.NetApiExt.Generated.Model.pallet_evm.account.EnumBasicCrossAccountIdRepr, U128>)commonEvent.Value2;

                                    tokenIdTask.TrySetResult(((TokenId)createdItemEvent.Value[1]).Value);

                                    return;
                                }
                            }
                        }

                        break;

                }
            };
#pragma warning restore VSTHRD101 // Avoid unsupported async delegates

            await substrateClient.Author.SubmitAndWatchExtrinsicAsync(
                callback,
                Utils.Bytes2HexString(unCheckedExtrinsic.Encode()),
                cancellationToken
            );

            var timeoutTask = Task.Delay(finalityTimeout, cancellationToken);

            if (await Task.WhenAny(tokenIdTask.Task, timeoutTask) == timeoutTask)
            {
                // If timeouted, set the result to null
                tokenIdTask.TrySetResult(null);
            }

            // Return the resulting Token Id
            return await tokenIdTask.Task;
        }

        /// <summary>
        /// Returns nft with the same Collection ID and NFT ID.
        /// </summary>
        /// <param name="collectionId"></param>
        /// <param name="nftId"></param>
        /// <param name="network">Network you want to use</param>
        /// <param name="token">cancellation token</param>
        /// <returns>Nft or null if no nft with the corresponding ids was found</returns>
        public static async Task<UniqueNftGraphQLEntity?> GetNftByIdAsync(
            int collectionId,
            int nftId,
            NetworkEnum? network = null,
            CancellationToken token = default
        )
        {
            // If network is not provided, use the default one
            network ??= SdkConfig.UseDefaultNetwork;

            var client = new GraphQLHttpClient(
                Constants.GetGraphQLUrl(network), new NewtonsoftJsonSerializer()
            );

            var filter = new { token_id = new { _eq = nftId }, collection_id = new { _eq = collectionId} };

            var nfts = await UniqueNftGraphQLService.GetNftEntitiesAsync(
                client,
                filter,
                1,
                0,
                token
            );

            if (!nfts.Any())
            {
                return null;
            }

            return nfts[0];
        }

        /// <summary>
        /// Returns list of nfts with the same Collection ID
        /// </summary>
        /// <param name="collectionId"></param>
        /// <param name="limit">Max number of nfts to query in the list.</param>
        /// <param name="offset"></param>
        /// <param name="network">Network you want to use</param>
        /// <param name="token">cancellation token</param>
        /// <returns>List of nfts</returns>
        public static async Task<List<UniqueNftGraphQLEntity>> GetNftListByCollectionIdAsync(
            int collectionId,
            int limit = 25,
            int offset = 0,
            NetworkEnum? network = null,
            CancellationToken token = default
        )
        {
            // If network is not provided, use the default one
            network ??= SdkConfig.UseDefaultNetwork;

            var client = new GraphQLHttpClient(
                Constants.GetGraphQLUrl(network), new NewtonsoftJsonSerializer()
            );

            var filter = new { collection_id = new { _eq = collectionId } };

            var nfts = await UniqueNftGraphQLService.GetNftEntitiesAsync(
                client,
                filter,
                limit,
                offset,
                token
            );

            return nfts;
        }

        /// <summary>
        /// Returns list of nfts filtered by collection name
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="limit">Max number of nfts to query in the list.</param>
        /// <param name="offset"></param>
        /// <param name="network">Network you want to use</param>
        /// <param name="token">cancellation token</param>
        /// <returns>List of nfts</returns>
        public static async Task<List<UniqueNftGraphQLEntity>> GetNftListByCollectionNameAsync(
            string collectionName,
            int limit = 25,
            int offset = 0,
            NetworkEnum? network = null,
            CancellationToken token = default
        )
        {
            // If network is not provided, use the default one
            network ??= SdkConfig.UseDefaultNetwork;

            var client = new GraphQLHttpClient(
                Constants.GetGraphQLUrl(network), new NewtonsoftJsonSerializer()
            );

            var filter = new { collection_name = new { _eq = collectionName } };

            var nfts = await UniqueNftGraphQLService.GetNftEntitiesAsync(
                client,
                filter,
                limit,
                offset,
                token
            );

            return nfts;
        }

        /// <summary>
        /// Returns list of nfts filtered by owner
        /// </summary>
        /// <param name="ownerAddress">SS58 encoded address of the owner (in any format)</param>
        /// <param name="limit">Max number of nfts to query in the list.</param>
        /// <param name="offset"></param>
        /// <param name="network">Network you want to use</param>
        /// <param name="token">cancellation token</param>
        /// <returns>List of nfts</returns>
        public static async Task<List<UniqueNftGraphQLEntity>> GetNftListByOwnerAsync(
            string ownerAddress,
            int limit = 25,
            int offset = 0,
            NetworkEnum? network = null,
            CancellationToken token = default
        )
        {
            // If network is not provided, use the default one
            network ??= SdkConfig.UseDefaultNetwork;

            var client = new GraphQLHttpClient(
                Constants.GetGraphQLUrl(network), new NewtonsoftJsonSerializer()
            );

            var filter = new { owner = new { _eq = Utils.GetAddressFrom(Utils.GetPublicKeyFrom(ownerAddress), Constants.GetSS58Prefix(network)) } };

            var nfts = await UniqueNftGraphQLService.GetNftEntitiesAsync(
                client,
                filter,
                limit,
                offset,
                token
            );

            return nfts;
        }
    }
}

