using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Rpc;
using Substrate.NetApiExt.Generated;
using Substrate.NetApiExt.Generated.Model.frame_system;

namespace UniqueSDK
{
	public static class ExtrinsicModel
	{
        /// <summary>
        /// Signs the provided extrinsic, submits it to the chain,
        /// and listens to on-chain events.
        /// </summary>
        /// <param name="substrateClient"></param>
        /// <param name="unCheckedExtrinsic"></param>
        /// <param name="waitForFinality">Set to false, if you do not want to await until the Finality is confirmed</param>
        /// <param name="customCallback"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Status of the submitted extrinsic</returns>
        public static async Task<ExtrinsicResult> SignAndSubmitExtrinsicAsync(
            this SubstrateClientExt substrateClient,
            UnCheckedExtrinsic unCheckedExtrinsic,
            bool waitForFinality = true,
            Action<string, ExtrinsicStatus>? customCallback = null,
            CancellationToken cancellationToken = default
        )
        {
            if (!waitForFinality)
            {
                await substrateClient.Author.SubmitExtrinsicAsync(
                    Utils.Bytes2HexString(unCheckedExtrinsic.Encode()),
                    cancellationToken
                );

                return ExtrinsicResult.Submitted;
            }

            var collectionIdTask = new TaskCompletionSource<ExtrinsicResult>();

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
                        Console.WriteLine("Invalid extrinsic");
                        collectionIdTask.TrySetResult(ExtrinsicResult.Failed);
                        break;

                    case ExtrinsicState.Finalized:

                        IEnumerable<EventRecord> allExtrinsicEvents;

                        try
                        {
                            allExtrinsicEvents = await EventsModel.GetExtrinsicEventsAsync(substrateClient, status.Hash, unCheckedExtrinsic);
                        }
                        catch
                        {
                            // Should never happen
                            collectionIdTask.TrySetResult(ExtrinsicResult.Unknown);

                            return;
                        }

                        foreach (var e in allExtrinsicEvents)
                        {
                            // Filter only Common.CollectionCreated events
                            if (e.Event.Value == Substrate.NetApiExt.Generated.Model.opal_runtime.RuntimeEvent.System)
                            {
                                var commonEvent = (Substrate.NetApiExt.Generated.Model.frame_system.pallet.EnumEvent)e.Event.Value2;

                                if (commonEvent.Value == Substrate.NetApiExt.Generated.Model.frame_system.pallet.Event.ExtrinsicSuccess)
                                {
                                    collectionIdTask.TrySetResult(ExtrinsicResult.Success);

                                    return;
                                }
                                else if (commonEvent.Value == Substrate.NetApiExt.Generated.Model.frame_system.pallet.Event.ExtrinsicFailed)
                                {
                                    collectionIdTask.TrySetResult(ExtrinsicResult.Failed);

                                    return;
                                }
                            }
                        }

                        // Should never happen
                        collectionIdTask.TrySetResult(ExtrinsicResult.Unknown);

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
                collectionIdTask.TrySetResult(ExtrinsicResult.TimedOut);
            }

            // Return the resulting Collection Id
            return await collectionIdTask.Task;
        }
    }

    public enum ExtrinsicResult
    {
        Success,
        Failed,
        Submitted,
        TimedOut,
        Unknown,
    }
}

