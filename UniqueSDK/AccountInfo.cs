using System.Numerics;

namespace UniqueSDK
{
    /// <summary>
    /// Account Info C# Wrapper
    /// </summary>
	public class AccountInfo
	{
        /// <summary>
        /// Account Info Constructor
        /// </summary>
        /// <param name="accountInfo"></param>
        /// <param name="decimals"></param>
        /// <param name="unit"></param>
        public AccountInfo(Opal.NetApiExt.Generated.Model.frame_system.AccountInfo accountInfo, uint decimals, string unit)
        {
            Nonce = accountInfo.Nonce.Value;
            Consumers = accountInfo.Consumers.Value;
            Providers = accountInfo.Providers.Value;
            Sufficients = accountInfo.Sufficients.Value;
            Data = new AccountData(accountInfo.Data, decimals, unit);
        }

        /// <summary>
        /// Nonce
        /// </summary>
        public uint Nonce { get; }

        /// <summary>
        /// Consumers
        /// </summary>
        public uint Consumers { get; }

        /// <summary>
        /// Providers
        /// </summary>
        public uint Providers { get; }

        /// <summary>
        /// Sufficients
        /// </summary>
        public uint Sufficients { get; }

        /// <summary>
        /// Data
        /// </summary>
        public AccountData Data { get; }
    }

    /// <summary>
    /// Account Data C# Wrapper
    /// </summary>
    public class AccountData
    {
        /// <summary>
        /// Account Data Constructor
        /// </summary>
        /// <param name="accountData"></param>
        public AccountData(Opal.NetApiExt.Generated.Model.pallet_balances.types.AccountData accountData, uint decimals, string unit)
        {
            Free = new Balance(accountData.Free.Value, decimals, unit);
            Reserved = new Balance(accountData.Reserved.Value, decimals, unit);
            Frozen = new Balance(accountData.Frozen.Value, decimals, unit);
            Flags = accountData.Flags.Value;
        }

        /// <summary>
        /// Free
        /// </summary>
        public Balance Free { get; }

        /// <summary>
        /// Reserved
        /// </summary>
        public Balance Reserved { get; }

        /// <summary>
        /// Frozen
        /// </summary>
        public Balance Frozen { get; }

        /// <summary>
        /// Flags
        /// </summary>
        public BigInteger Flags { get; }
    }
}

