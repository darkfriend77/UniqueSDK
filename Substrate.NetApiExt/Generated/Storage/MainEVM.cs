//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Meta;
using Substrate.NetApi.Model.Types;
using Substrate.NetApi.Model.Types.Base;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace Substrate.NetApiExt.Generated.Storage
{
    
    
    /// <summary>
    /// >> EVMStorage
    /// </summary>
    public sealed class EVMStorage
    {
        
        // Substrate client for the storage calls.
        private SubstrateClientExt _client;
        
        /// <summary>
        /// >> EVMStorage Constructor
        /// </summary>
        public EVMStorage(SubstrateClientExt client)
        {
            this._client = client;
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("EVM", "AccountCodes"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat}, typeof(Substrate.NetApiExt.Generated.Model.primitive_types.H160), typeof(Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8>)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("EVM", "AccountCodesMetadata"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat}, typeof(Substrate.NetApiExt.Generated.Model.primitive_types.H160), typeof(Substrate.NetApiExt.Generated.Model.pallet_evm.CodeMetadata)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("EVM", "AccountStorages"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat,
                            Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat}, typeof(Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApiExt.Generated.Model.primitive_types.H160, Substrate.NetApiExt.Generated.Model.primitive_types.H256>), typeof(Substrate.NetApiExt.Generated.Model.primitive_types.H256)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("EVM", "CurrentLogs"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(null, null, typeof(Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.EthereumLog>)));
        }
        
        /// <summary>
        /// >> AccountCodesParams
        /// </summary>
        public static string AccountCodesParams(Substrate.NetApiExt.Generated.Model.primitive_types.H160 key)
        {
            return RequestGenerator.GetStorage("EVM", "AccountCodes", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> AccountCodesDefault
        /// Default value as hex string
        /// </summary>
        public static string AccountCodesDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> AccountCodes
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8>> AccountCodes(Substrate.NetApiExt.Generated.Model.primitive_types.H160 key, string blockhash, CancellationToken token)
        {
            string parameters = EVMStorage.AccountCodesParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8>>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> AccountCodesMetadataParams
        /// </summary>
        public static string AccountCodesMetadataParams(Substrate.NetApiExt.Generated.Model.primitive_types.H160 key)
        {
            return RequestGenerator.GetStorage("EVM", "AccountCodesMetadata", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> AccountCodesMetadataDefault
        /// Default value as hex string
        /// </summary>
        public static string AccountCodesMetadataDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> AccountCodesMetadata
        /// </summary>
        public async Task<Substrate.NetApiExt.Generated.Model.pallet_evm.CodeMetadata> AccountCodesMetadata(Substrate.NetApiExt.Generated.Model.primitive_types.H160 key, string blockhash, CancellationToken token)
        {
            string parameters = EVMStorage.AccountCodesMetadataParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApiExt.Generated.Model.pallet_evm.CodeMetadata>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> AccountStoragesParams
        /// </summary>
        public static string AccountStoragesParams(Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApiExt.Generated.Model.primitive_types.H160, Substrate.NetApiExt.Generated.Model.primitive_types.H256> key)
        {
            return RequestGenerator.GetStorage("EVM", "AccountStorages", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat,
                        Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat}, key.Value);
        }
        
        /// <summary>
        /// >> AccountStoragesDefault
        /// Default value as hex string
        /// </summary>
        public static string AccountStoragesDefault()
        {
            return "0x0000000000000000000000000000000000000000000000000000000000000000";
        }
        
        /// <summary>
        /// >> AccountStorages
        /// </summary>
        public async Task<Substrate.NetApiExt.Generated.Model.primitive_types.H256> AccountStorages(Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApiExt.Generated.Model.primitive_types.H160, Substrate.NetApiExt.Generated.Model.primitive_types.H256> key, string blockhash, CancellationToken token)
        {
            string parameters = EVMStorage.AccountStoragesParams(key);
            var result = await _client.GetStorageAsync<Substrate.NetApiExt.Generated.Model.primitive_types.H256>(parameters, blockhash, token);
            return result;
        }
        
        /// <summary>
        /// >> CurrentLogsParams
        ///  Written on log, reset after transaction
        ///  Should be empty between transactions
        /// </summary>
        public static string CurrentLogsParams()
        {
            return RequestGenerator.GetStorage("EVM", "CurrentLogs", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
        }
        
        /// <summary>
        /// >> CurrentLogsDefault
        /// Default value as hex string
        /// </summary>
        public static string CurrentLogsDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> CurrentLogs
        ///  Written on log, reset after transaction
        ///  Should be empty between transactions
        /// </summary>
        public async Task<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.EthereumLog>> CurrentLogs(string blockhash, CancellationToken token)
        {
            string parameters = EVMStorage.CurrentLogsParams();
            var result = await _client.GetStorageAsync<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.EthereumLog>>(parameters, blockhash, token);
            return result;
        }
    }
    
    /// <summary>
    /// >> EVMCalls
    /// </summary>
    public sealed class EVMCalls
    {
        
        /// <summary>
        /// >> withdraw
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method Withdraw(Substrate.NetApiExt.Generated.Model.primitive_types.H160 address, Substrate.NetApi.Model.Types.Primitive.U128 value)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(address.Encode());
            byteArray.AddRange(value.Encode());
            return new Method(100, "EVM", 0, "withdraw", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> call
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method Call(Substrate.NetApiExt.Generated.Model.primitive_types.H160 source, Substrate.NetApiExt.Generated.Model.primitive_types.H160 target, Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8> input, Substrate.NetApiExt.Generated.Model.primitive_types.U256 value, Substrate.NetApi.Model.Types.Primitive.U64 gas_limit, Substrate.NetApiExt.Generated.Model.primitive_types.U256 max_fee_per_gas, Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApiExt.Generated.Model.primitive_types.U256> max_priority_fee_per_gas, Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApiExt.Generated.Model.primitive_types.U256> nonce, Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApiExt.Generated.Model.primitive_types.H160, Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.primitive_types.H256>>> access_list)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(source.Encode());
            byteArray.AddRange(target.Encode());
            byteArray.AddRange(input.Encode());
            byteArray.AddRange(value.Encode());
            byteArray.AddRange(gas_limit.Encode());
            byteArray.AddRange(max_fee_per_gas.Encode());
            byteArray.AddRange(max_priority_fee_per_gas.Encode());
            byteArray.AddRange(nonce.Encode());
            byteArray.AddRange(access_list.Encode());
            return new Method(100, "EVM", 1, "call", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> create
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method Create(Substrate.NetApiExt.Generated.Model.primitive_types.H160 source, Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8> init, Substrate.NetApiExt.Generated.Model.primitive_types.U256 value, Substrate.NetApi.Model.Types.Primitive.U64 gas_limit, Substrate.NetApiExt.Generated.Model.primitive_types.U256 max_fee_per_gas, Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApiExt.Generated.Model.primitive_types.U256> max_priority_fee_per_gas, Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApiExt.Generated.Model.primitive_types.U256> nonce, Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApiExt.Generated.Model.primitive_types.H160, Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.primitive_types.H256>>> access_list)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(source.Encode());
            byteArray.AddRange(init.Encode());
            byteArray.AddRange(value.Encode());
            byteArray.AddRange(gas_limit.Encode());
            byteArray.AddRange(max_fee_per_gas.Encode());
            byteArray.AddRange(max_priority_fee_per_gas.Encode());
            byteArray.AddRange(nonce.Encode());
            byteArray.AddRange(access_list.Encode());
            return new Method(100, "EVM", 2, "create", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> create2
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method Create2(Substrate.NetApiExt.Generated.Model.primitive_types.H160 source, Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8> init, Substrate.NetApiExt.Generated.Model.primitive_types.H256 salt, Substrate.NetApiExt.Generated.Model.primitive_types.U256 value, Substrate.NetApi.Model.Types.Primitive.U64 gas_limit, Substrate.NetApiExt.Generated.Model.primitive_types.U256 max_fee_per_gas, Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApiExt.Generated.Model.primitive_types.U256> max_priority_fee_per_gas, Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApiExt.Generated.Model.primitive_types.U256> nonce, Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApiExt.Generated.Model.primitive_types.H160, Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApiExt.Generated.Model.primitive_types.H256>>> access_list)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(source.Encode());
            byteArray.AddRange(init.Encode());
            byteArray.AddRange(salt.Encode());
            byteArray.AddRange(value.Encode());
            byteArray.AddRange(gas_limit.Encode());
            byteArray.AddRange(max_fee_per_gas.Encode());
            byteArray.AddRange(max_priority_fee_per_gas.Encode());
            byteArray.AddRange(nonce.Encode());
            byteArray.AddRange(access_list.Encode());
            return new Method(100, "EVM", 3, "create2", byteArray.ToArray());
        }
    }
    
    /// <summary>
    /// >> EVMConstants
    /// </summary>
    public sealed class EVMConstants
    {
    }
    
    /// <summary>
    /// >> EVMErrors
    /// </summary>
    public enum EVMErrors
    {
        
        /// <summary>
        /// >> BalanceLow
        /// Not enough balance to perform action
        /// </summary>
        BalanceLow,
        
        /// <summary>
        /// >> FeeOverflow
        /// Calculating total fee overflowed
        /// </summary>
        FeeOverflow,
        
        /// <summary>
        /// >> PaymentOverflow
        /// Calculating total payment overflowed
        /// </summary>
        PaymentOverflow,
        
        /// <summary>
        /// >> WithdrawFailed
        /// Withdraw fee failed
        /// </summary>
        WithdrawFailed,
        
        /// <summary>
        /// >> GasPriceTooLow
        /// Gas price is too low.
        /// </summary>
        GasPriceTooLow,
        
        /// <summary>
        /// >> InvalidNonce
        /// Nonce is invalid
        /// </summary>
        InvalidNonce,
        
        /// <summary>
        /// >> GasLimitTooLow
        /// Gas limit is too low.
        /// </summary>
        GasLimitTooLow,
        
        /// <summary>
        /// >> GasLimitTooHigh
        /// Gas limit is too high.
        /// </summary>
        GasLimitTooHigh,
        
        /// <summary>
        /// >> Undefined
        /// Undefined error.
        /// </summary>
        Undefined,
        
        /// <summary>
        /// >> Reentrancy
        /// EVM reentrancy
        /// </summary>
        Reentrancy,
        
        /// <summary>
        /// >> TransactionMustComeFromEOA
        /// EIP-3607,
        /// </summary>
        TransactionMustComeFromEOA,
    }
}
