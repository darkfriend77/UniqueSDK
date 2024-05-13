using System.Xml.Linq;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Rpc;
using Substrate.NetApi.Model.Types;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;
using Opal.NetApiExt.Generated;
using Opal.NetApiExt.Generated.Model.frame_system;
using Opal.NetApiExt.Generated.Model.up_data_structs;

namespace UniqueSDK
{
    public static class CollectionModel
    {
        /// <summary>
        /// https://rest.unique.network/opal/swagger#/collections/createCollectionMutationSchemaV2
        /// </summary>
        /// <param name="collection">Collection data</param>
        /// <param name="nonce">Nonce of the account</param>
        /// <param name="network">Network you want to use</param>
        /// <param name="use"></param>
        /// <param name="withFee"></param>
        /// <param name="verify"></param>
        /// <param name="callbackUrl"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
		public static async Task<RestResponse> CreateCollectionRestAsync(
            UniqueCollectionRest collection,
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

            var url = $"{Constants.GetRestUrl(network)}/v1/collections/v2?use={use}&withFee={withFee}&verify={verify}&nonce={nonce}{callback}";

            return await RestModel.ExecutePostAsync(url, collection, cancellationToken);
        }

        /// <summary>
        /// Constructs the Unique.createCollectionEx extrinsic, signs it, submits it to the chain,
        /// listens to on-chain events and filters the events for Common.CollectionCreated
        /// to get the CollectionId of the newly created Collection.
        /// </summary>
        /// <param name="substrateClient"></param>
        /// <param name="account"></param>
        /// <param name="collection"></param>
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
        /// <returns>Collection Id of the newly created Collection</returns>
        public static async Task<uint?> SignAndSubmitCreateCollectionExtrinsicAsync(
            this SubstrateClientExt substrateClient,
            Account account,
            UniqueCollectionRest collection,
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

            var response = await CollectionModel.CreateCollectionRestAsync(
                collection,
                nonce.Value,
                network,
                use,
                withFee,
                verify,
                callbackUrl,
                cancellationToken
            );

            return await substrateClient.SignAndSubmitCreateCollectionExtrinsicAsync(
                account,
                response,
                customCallback,
                signed,
                finalityTimeout,
                cancellationToken
            );
        }

        /// <summary>
        /// Constructs the Unique.createCollectionEx extrinsic, signs it, submits it to the chain,
        /// listens to on-chain events and filters the events for Common.CollectionCreated
        /// to get the CollectionId of the newly created Collection.
        /// </summary>
        /// <param name="substrateClient"></param>
        /// <param name="account"></param>
        /// <param name="response"></param>
        /// <param name="customCallback"></param>
        /// <param name="signed"></param>
        /// <param name="finalityTimeout">The maximum amount of time to wait for the finality. Defaultly wait maximum 60 seconds.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Collection Id of the newly created Collection</returns>
        public static async Task<uint?> SignAndSubmitCreateCollectionExtrinsicAsync(
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

            var collectionIdTask = new TaskCompletionSource<uint?>();

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
                        collectionIdTask.TrySetResult(null);
                        break;

                    case ExtrinsicState.Finalized:

                        IEnumerable<EventRecord> allExtrinsicEvents;

                        try
                        {
                            allExtrinsicEvents = await EventsModel.GetExtrinsicEventsAsync(substrateClient, status.Hash, unCheckedExtrinsic);
                        }
                        catch
                        {
                            collectionIdTask.TrySetResult(null);

                            return;
                        }

                        foreach (var e in allExtrinsicEvents)
                        {
                            // Filter only Common.CollectionCreated events
                            if (e.Event.Value == Opal.NetApiExt.Generated.Model.opal_runtime.RuntimeEvent.Common)
                            {
                                var commonEvent = (Opal.NetApiExt.Generated.Model.pallet_common.pallet.EnumEvent)e.Event.Value2;

                                if (commonEvent.Value == Opal.NetApiExt.Generated.Model.pallet_common.pallet.Event.CollectionCreated)
                                {
                                    var createdCollectionEvent = (BaseTuple<CollectionId, U8, Opal.NetApiExt.Generated.Model.sp_core.crypto.AccountId32>)commonEvent.Value2;

                                    collectionIdTask.TrySetResult(((CollectionId)createdCollectionEvent.Value[0]).Value);

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

            if (await Task.WhenAny(collectionIdTask.Task, timeoutTask) == timeoutTask)
            {
                // If timeouted, set the result to null
                collectionIdTask.TrySetResult(null);
            }

            // Return the resulting Collection Id
            return await collectionIdTask.Task;
        }

        /// <summary>
        /// Returns collection by id.
        /// </summary>
        /// <param name="id">collection id</param>
        /// <param name="network">Network you want to use</param>
        /// <param name="token">cancellation token</param>
        /// <returns>Collection or null if no collection was found</returns>
        public static async Task<UniqueCollectionGraphQLEntity?> GetCollectionByIdAsync(
            int id,
            NetworkEnum? network = null,
            CancellationToken token = default
        )
        {
            // If network is not provided, use the default one
            network ??= SdkConfig.UseDefaultNetwork;

            var client = new GraphQLHttpClient(
                Constants.GetGraphQLUrl(network), new NewtonsoftJsonSerializer()
            );

            var filter = new { collection_id = new { _eq = id } };

            var collections = await UniqueCollectionGraphQLService.GetCollectionEntitiesAsync(
                client,
                filter,
                1,
                0,
                token
            );

            if (!collections.Any())
            {
                return null;
            }

            return collections[0];
        }

        /// <summary>
        /// Returns list of collections filtered by name
        /// </summary>
        /// <param name="name">Full name</param>
        /// <param name="limit">Max number of collections to query in the list.</param>
        /// <param name="offset"></param>
        /// <param name="network">Network you want to use</param>
        /// <param name="token">Cancellation token</param>
        /// <returns>List of collections</returns>
        public static async Task<List<UniqueCollectionGraphQLEntity>> GetCollectionsByNameAsync(
           string name,
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

            var filter = new { name = new { _eq = name } };

            var collections = await UniqueCollectionGraphQLService.GetCollectionEntitiesAsync(
                client,
                filter,
                limit,
                offset,
                token
            );

            return collections;
        }

        /// <summary>
        /// Returns list of collections filtered by owner
        /// </summary>
        /// <param name="ownerAddress">SS58 encoded address of the owner (in any format)</param>
        /// <param name="limit">Max number of collections to query in the list.</param>
        /// <param name="offset"></param>
        /// <param name="network">Network you want to use</param>
        /// <param name="token">Cancellation token</param>
        /// <returns>List of collections</returns>
        public static async Task<List<UniqueCollectionGraphQLEntity>> GetCollectionsByOwnerAsync(
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

            var collections = await UniqueCollectionGraphQLService.GetCollectionEntitiesAsync(
                client,
                filter,
                limit,
                offset,
                token
            );

            return collections;
        }
    }
}

