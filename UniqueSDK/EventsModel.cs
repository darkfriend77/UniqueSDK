using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Rpc;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;
using Substrate.NetApiExt.Generated;
using Substrate.NetApiExt.Generated.Model.frame_system;

namespace UniqueSDK
{
    public static class EventsModel
    {
        /// <summary>
        /// Gets all extrinsic events in the block
        /// </summary>
        /// <param name="substrateClient"></param>
        /// <param name="blockHash"></param>
        /// <param name="unCheckedExtrinsic"></param>
        /// <returns>all events for the given extrinsic</returns>
        /// <exception cref="ExtrinsicIndexNotFoundException"></exception>
        public static async Task<IEnumerable<EventRecord>> GetExtrinsicEventsAsync(
            this SubstrateClientExt substrateClient,
            Hash blockHash,
            UnCheckedExtrinsic unCheckedExtrinsic
        )
        {
            var events = await substrateClient.SystemStorage.Events(blockHash.Value, CancellationToken.None);

            BlockData block = await substrateClient.Chain.GetBlockAsync(blockHash, CancellationToken.None);

            int? extrinsicIndex = null;
            for (int i = 0; i < block.Block.Extrinsics.Count(); i++)
            {
                var extrinsic = block.Block.Extrinsics[i];

                if (Utils.Bytes2HexString(extrinsic.Encode()).Equals(Utils.Bytes2HexString(unCheckedExtrinsic.Encode())))
                {
                    extrinsicIndex = i;
                    break;
                }
            };

            if (extrinsicIndex is null)
            {
                throw new ExtrinsicIndexNotFoundException();
            }

            return events.Value.Where(p => p.Phase.Value == Phase.ApplyExtrinsic && ((U32)p.Phase.Value2).Value == extrinsicIndex);
        }
    }
}

