using Substrate.NetApi;
using Substrate.NetApi.Model.Types;
using Substrate.Opal.NET.NetApiExt.Generated.Model.sp_core.crypto;

namespace UniqueSDK
{
	public static class GenericHelpers
	{
        /// <summary>
        /// Convert address string to public key
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static byte[] ToPublicKey(this string address)
        {
            return Utils.GetPublicKeyFrom(address);
        }

        /// <summary>
        /// Convert address string to AccountId32
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static AccountId32 ToAccountId32(this string address)
        {
            var account32 = new AccountId32();
            account32.Create(address.ToPublicKey());

            return account32;
        }

        /// <summary>
        /// Convert Account to AccountId32
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static AccountId32 ToAccountId32(this Account account)
        {
            var account32 = new AccountId32();
            account32.Create(account.Bytes);

            return account32;
        }
    }
}

