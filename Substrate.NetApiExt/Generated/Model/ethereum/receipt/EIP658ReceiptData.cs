//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Substrate.NetApi.Attributes;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Metadata.V14;
using System.Collections.Generic;


namespace Substrate.NetApiExt.Generated.Model.ethereum.receipt
{
    
    
    /// <summary>
    /// >> 650 - Composite[ethereum.receipt.EIP658ReceiptData]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class EIP658ReceiptData : BaseType
    {
        
        /// <summary>
        /// >> status_code
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U8 StatusCode { get; set; }
        /// <summary>
        /// >> used_gas
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.primitive_types.U256 UsedGas { get; set; }
        /// <summary>
        /// >> logs_bloom
        /// </summary>
        public Substrate.NetApiExt.Generated.Model.ethbloom.Bloom LogsBloom { get; set; }
        /// <summary>
        /// >> logs
        /// </summary>
        public Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.ethereum.log.EthereumLog> Logs { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "EIP658ReceiptData";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(StatusCode.Encode());
            result.AddRange(UsedGas.Encode());
            result.AddRange(LogsBloom.Encode());
            result.AddRange(Logs.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            StatusCode = new Substrate.NetApi.Model.Types.Primitive.U8();
            StatusCode.Decode(byteArray, ref p);
            UsedGas = new Substrate.NetApiExt.Generated.Model.primitive_types.U256();
            UsedGas.Decode(byteArray, ref p);
            LogsBloom = new Substrate.NetApiExt.Generated.Model.ethbloom.Bloom();
            LogsBloom.Decode(byteArray, ref p);
            Logs = new Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.ethereum.log.EthereumLog>();
            Logs.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
