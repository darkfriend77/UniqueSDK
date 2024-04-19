using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Types;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Rpc;
using System.Globalization;
using System.Numerics;
using System.Net;

namespace UniqueSDK
{
    public class Response
    {
        public SignerPayloadJSON SignerPayloadJSON { get; set; }
        public SignerPayloadRaw SignerPayloadRaw { get; set; }
        public string SignerPayloadHex { get; set; }
        public Balance Fee { get; set; }
    }

    public class SignerPayloadJSON
    {
        public string SpecVersion { get; set; }
        public string TransactionVersion { get; set; }
        public string Address { get; set; }
        public string BlockHash { get; set; }
        public string BlockNumber { get; set; }
        public string Era { get; set; }
        public string GenesisHash { get; set; }
        public string Method { get; set; }
        public string Nonce { get; set; }
        public List<string> SignedExtensions { get; set; }
        public string Tip { get; set; }
        public uint Version { get; set; }

        /// <summary>
        /// Remakes the json extrinsic representation into the Substrate.Net.Api native UncheckedExtrinsic.
        /// If signed argument is true, it signs the extrinsic with the provided account.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="signed"></param>
        /// <returns>UnCheckedExtrinsic</returns>
        /// <exception cref="SignerPayloadJSONMissingPropertiesException"></exception>
        public async Task<UnCheckedExtrinsic> ToExtrinsicAsync(Account account, bool signed = true)
        {
            byte[] methodBytes = Utils.HexToByteArray(Method);

            List<byte> methodParameters = new List<byte>();

            for (int i = 2; i < methodBytes.Length; i++)
            {
                methodParameters.Add(methodBytes[i]);
            }

            Method substrateMethod = new Method(
                methodBytes[0],
                methodBytes[1],
                methodParameters.ToArray()
            );

            if (Tip is null || SpecVersion is null ||
                    TransactionVersion is null || Nonce is null)
            {
                throw new SignerPayloadJSONMissingPropertiesException();
            }

            Hash substrateBlockHash = new Hash();
            substrateBlockHash.Create(BlockHash);

            Hash substrateGenesisHash = new Hash();
            substrateGenesisHash.Create(Utils.HexToByteArray(GenesisHash));

            // There is an assumption that we are using ChargeTransactionPayment
            ChargeType charge = new ChargeTransactionPayment(0);
            int p = 0;
            charge.Decode(Utils.HexToByteArray(Tip), ref p);

            RuntimeVersion runtimeVersion = new RuntimeVersion
            {
                ImplVersion = Version,
                SpecVersion = (uint)Utils.Bytes2Value(Utils.HexToByteArray(SpecVersion), littleEndian: false),
                TransactionVersion = (uint)Utils.Bytes2Value(Utils.HexToByteArray(TransactionVersion), littleEndian: false),
            };

            uint.TryParse(
                Nonce.Replace("0x", ""),
                NumberStyles.HexNumber,
                CultureInfo.InvariantCulture,
                out uint nonceUint
            );

            return await RequestGenerator.SubmitExtrinsicAsync(
                signed,
                account,
                substrateMethod,
                Substrate.NetApi.Model.Extrinsics.Era.Decode(Utils.HexToByteArray(Era)),
                nonceUint,
                charge,
                substrateGenesisHash,
                substrateBlockHash,
                runtimeVersion);
        }
    }

    public class SignerPayloadRaw
    {
        public string Address { get; set; }
        public string Data { get; set; }
        public string Type { get; set; }
    }
}

