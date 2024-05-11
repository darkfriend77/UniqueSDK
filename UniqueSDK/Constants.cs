namespace UniqueSDK
{
	public class Constants
	{
        private const string OPAL_REST_URL = "https://rest.unique.network/opal";
        public const string OPAL_NODE_URL = "wss://ws-opal.unique.network";
        private const string OPAL_GRAPHQL_URL = "https://api-opal.uniquescan.io/v1/graphql";
        private const string OPAL_UNIT = "OPL";
        private const uint OPAL_DECIMALS = 18;
        private const short OPAL_SS58_PREFIX = 42;

        private const string UNIQUE_REST_URL = "https://rest.unique.network/unique";
        public const string UNIQUE_NODE_URL = "wss://ws.unique.network";
        private const string UNIQUE_GRAPHQL_URL = "https://api-unique.uniquescan.io/v1/graphql";
        private const string UNIQUE_UNIT = "UNQ";
        private const uint UNIQUE_DECIMALS = 18;
        private const short UNIQUE_SS58_PREFIX = 7391;

        public static string GetRestUrl(NetworkEnum? network)
        {
            switch (network)
            {
                case NetworkEnum.Opal:
                    return OPAL_REST_URL;
                case NetworkEnum.Unique:
                    return UNIQUE_REST_URL;
            }

            return UNIQUE_REST_URL;
        }

        public static string GetNodeUrl(NetworkEnum? network)
        {
            switch (network)
            {
                case NetworkEnum.Opal:
                    return OPAL_NODE_URL;
                case NetworkEnum.Unique:
                    return UNIQUE_NODE_URL;
            }

            return UNIQUE_NODE_URL;
        }

        public static string GetGraphQLUrl(NetworkEnum? network)
        {
            switch (network)
            {
                case NetworkEnum.Opal:
                    return OPAL_GRAPHQL_URL;
                case NetworkEnum.Unique:
                    return UNIQUE_GRAPHQL_URL;
            }

            return UNIQUE_GRAPHQL_URL;
        }

        public static string GetUnit(NetworkEnum? network)
        {
            switch (network)
            {
                case NetworkEnum.Opal:
                    return OPAL_UNIT;
                case NetworkEnum.Unique:
                    return UNIQUE_UNIT;
            }

            return UNIQUE_UNIT;
        }

        public static uint GetDecimals(NetworkEnum? network)
        {
            switch (network)
            {
                case NetworkEnum.Opal:
                    return OPAL_DECIMALS;
                case NetworkEnum.Unique:
                    return UNIQUE_DECIMALS;
            }

            return UNIQUE_DECIMALS;
        }

        public static short GetSS58Prefix(NetworkEnum? network)
        {
            switch (network)
            {
                case NetworkEnum.Opal:
                    return OPAL_SS58_PREFIX;
                case NetworkEnum.Unique:
                    return UNIQUE_SS58_PREFIX;
            }

            return UNIQUE_SS58_PREFIX;
        }
    }
}

