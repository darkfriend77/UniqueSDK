using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Types;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Rpc;
using System.Globalization;

namespace UniqueSDK
{
    public class Response
    {
        public SignerPayloadJSON signerPayloadJSON { get; set; }
        public SignerPayloadRaw signerPayloadRaw { get; set; }
        public string signerPayloadHex { get; set; }
        public Fee fee { get; set; }
    }

    public class SignerPayloadJSON
    {
        public string specVersion { get; set; }
        public string transactionVersion { get; set; }
        public string address { get; set; }
        public string blockHash { get; set; }
        public string blockNumber { get; set; }
        public string era { get; set; }
        public string genesisHash { get; set; }
        public string method { get; set; }
        public string nonce { get; set; }
        public List<string> signedExtensions { get; set; }
        public string tip { get; set; }
        public uint version { get; set; }

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
            byte[] methodBytes = Utils.HexToByteArray(method);

            List<byte> methodParameters = new List<byte>();

            for (int i = 2; i < methodBytes.Length; i++)
            {
                methodParameters.Add(methodBytes[i]);
            }

            Method substrateMethod = new Method(methodBytes[0], methodBytes[1], methodParameters.ToArray());

            if (tip is null || specVersion is null ||
                    transactionVersion is null || nonce is null)
            {
                throw new SignerPayloadJSONMissingPropertiesException();
            }

            Hash substrateBlockHash = new Hash();
            substrateBlockHash.Create(blockHash);

            Hash substrateGenesisHash = new Hash();
            substrateGenesisHash.Create(Utils.HexToByteArray(genesisHash));

            // There is an assumption that we are using ChargeTransactionPayment
            ChargeType charge = new ChargeTransactionPayment(0);
            int p = 0;
            charge.Decode(Utils.HexToByteArray(tip), ref p);

            RuntimeVersion runtimeVersion = new RuntimeVersion
            {
                ImplVersion = version,
                SpecVersion = (uint)Utils.Bytes2Value(Utils.HexToByteArray(specVersion), littleEndian: false),
                TransactionVersion = (uint)Utils.Bytes2Value(Utils.HexToByteArray(transactionVersion), littleEndian: false),
            };

            // remove the 0x if it's there
            uint.TryParse(nonce.Replace("0x", ""), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint nonceUint);

            return await RequestGenerator.SubmitExtrinsicAsync(
                signed,
                account,
                substrateMethod,
                Era.Decode(Utils.HexToByteArray(era)),
                nonceUint,
                charge,
                substrateGenesisHash,
                substrateBlockHash,
                runtimeVersion);
        }
    }

    public class SignerPayloadRaw
    {
        public string address { get; set; }
        public string data { get; set; }
        public string type { get; set; }
    }

    public class Fee
    {
        public string raw { get; set; }
        public string amount { get; set; }
        public string formatted { get; set; }
        public uint decimals { get; set; }
        public string unit { get; set; }
    }
}
