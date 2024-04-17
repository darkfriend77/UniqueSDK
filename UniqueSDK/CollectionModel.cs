using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Rpc;
using Substrate.NetApi.Model.Types;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;
using Substrate.NetApiExt.Generated;
using Substrate.NetApiExt.Generated.Model.frame_system;
using Substrate.NetApiExt.Generated.Model.up_data_structs;

namespace UniqueSDK
{
    public class CollectionModel
    {
        /// <summary>
        /// https://rest.unique.network/opal/swagger#/collections/createCollectionMutationSchemaV2
        /// </summary>
        /// <param name="collection">Collection data</param>
        /// <param name="nonce">Nonce of the account</param>
        /// <param name="use"></param>
        /// <param name="withFee"></param>
        /// <param name="verify"></param>
        /// <param name="callbackUrl"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
		public static async Task<Response> CreateCollectionRestAsync(
            Collection collection,
            uint nonce,
            UseEnum use = UseEnum.Build,
            bool withFee = false,
            bool verify = false,
            string? callbackUrl = null
        )
        {
            string callback = callbackUrl is null ? "" : $"&callbackUrl={callbackUrl}"; // Handle string encoding

            var url = $"{Constants.OPAL_REST_URL}/collections/v2?use={use}&withFee={withFee}&verify={verify}&nonce={nonce}{callback}";

            var client = new HttpClient();

            string json = System.Text.Json.JsonSerializer.Serialize(
                collection,
                new System.Text.Json.JsonSerializerOptions
                {
                    // Ignore all default / null properties
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
                }
            );

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );

            // Add headers
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                var response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<Response>(responseContent) ?? new Response();
                }
                else
                {
                    Console.WriteLine("Request failed.");
                    Console.WriteLine($"Status Code: {response.StatusCode}");
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
            }

            // Handle the Exception better
            throw new Exception("Rest request failed");
        }

        /// <summary>
        /// Constructs the CreateCollectionEx extrinsic, signs it, submits it to the chain,
        /// listens to on-chain events and filters the events for Common.CollectionCreated
        /// to get the CollectionId of the newly created Collection.
        /// </summary>
        /// <param name="substrateClient"></param>
        /// <param name="account"></param>
        /// <param name="collection"></param>
        /// <param name="customCallback"></param>
        /// <param name="nonce"></param>
        /// <param name="use"></param>
        /// <param name="withFee"></param>
        /// <param name="verify"></param>
        /// <param name="callbackUrl"></param>
        /// <param name="signed"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Collection Id of the newly created Collection</returns>
        public static async Task<uint?> SignAndSubmitCreateCollectionExtrinsicAsync(
            SubstrateClientExt substrateClient,
            Account account,
            Collection collection,
            Action<string, ExtrinsicStatus>? customCallback = null,
            uint? nonce = null,
            UseEnum use = UseEnum.Build,
            bool withFee = false,
            bool verify = false,
            string? callbackUrl = null,
            bool signed = true,
            CancellationToken cancellationToken = default
        )
        {
            // If nonce is not provided, get a new one
            nonce ??= await substrateClient.System.AccountNextIndexAsync(account.Value, cancellationToken);

            Response response = await CollectionModel.CreateCollectionRestAsync(
                collection,
                nonce.Value,
                use,
                withFee,
                verify,
                callbackUrl
            );

            return await SignAndSubmitCreateCollectionExtrinsicAsync(
                substrateClient,
                account,
                response,
                customCallback,
                signed,
                cancellationToken
            );
        }

        /// <summary>
        /// Constructs the CreateCollectionEx extrinsic, signs it, submits it to the chain,
        /// listens to on-chain events and filters the events for Common.CollectionCreated
        /// to get the CollectionId of the newly created Collection.
        /// </summary>
        /// <param name="substrateClient"></param>
        /// <param name="account"></param>
        /// <param name="response"></param>
        /// <param name="customCallback"></param>
        /// <param name="signed"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Collection Id of the newly created Collection</returns>
        public static async Task<uint?> SignAndSubmitCreateCollectionExtrinsicAsync(
            SubstrateClientExt substrateClient,
            Account account,
            Response response,
            Action<string, ExtrinsicStatus>? customCallback = null,
            bool signed = true,
            CancellationToken cancellationToken = default
        )
        {
            UnCheckedExtrinsic unCheckedExtrinsic = await response.signerPayloadJSON.ToExtrinsicAsync(account, signed);

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
                            if (e.Event.Value == Substrate.NetApiExt.Generated.Model.opal_runtime.RuntimeEvent.Common)
                            {
                                var commonEvent = (Substrate.NetApiExt.Generated.Model.pallet_common.pallet.EnumEvent)e.Event.Value2;

                                if (commonEvent.Value == Substrate.NetApiExt.Generated.Model.pallet_common.pallet.Event.CollectionCreated)
                                {
                                    var createdCollectionEvent = (BaseTuple<CollectionId, U8, Substrate.NetApiExt.Generated.Model.sp_core.crypto.AccountId32>)commonEvent.Value2;

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

            // 60 seconds timeout
            var timeoutTask = Task.Delay(60_000, cancellationToken);

            if (await Task.WhenAny(collectionIdTask.Task, timeoutTask) == timeoutTask)
            {
                // If timeouted, set the result to null
                collectionIdTask.TrySetResult(null);
            }

            // Return the resulting Collection Id
            return await collectionIdTask.Task;
        }
    }

    public enum UseEnum
    {
        Build,
        BuildSequence,
        Sign,
        Submit,
        Result,
        GetFee,
    }
}

