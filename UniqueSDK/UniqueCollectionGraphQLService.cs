﻿using System;
using System.Text.Json.Serialization;
using GraphQL;
using GraphQL.Client.Http;

namespace UniqueSDK
{
    public class UniqueCollectionGraphQLService
    {
        private class ResponseType
        {
            [JsonPropertyName("collections")]
            public CollectionsWrapper Collections { get; set; }
        }

        private class CollectionsWrapper
        {
            [JsonPropertyName("data")]
            public List<UniqueCollectionGraphQLEntity> Data { get; set; }
        }

        public static async Task<List<UniqueCollectionGraphQLEntity>> GetCollectionEntitiesAsync(
            GraphQLHttpClient client,
            object filter,
            int limit = 25,
            int offset = 0,
            CancellationToken token = default
        )
        {
            GraphQLRequest request = new GraphQLRequest
            {
                Query = @"
                    query MyQuery ($limit: Int, $offset: Int, $where: CollectionWhereParams) {
                      collections(limit: $limit, offset: $offset, where: $where, order_by: { date_of_creation: desc }){
                        data {
                          actions_count
                          attributes_schema
                          burned
                          collection_cover
                          collection_id
                          const_chain_schema
                          date_of_creation
                          description
                          holders_count
                          limits_account_ownership
                          limits_sponsore_data_rate
                          limits_sponsore_data_size
                          mint_mode
                          mode
                          name
                          nesting_enabled
                          offchain_schema
                          owner
                          owner_can_destroy
                          owner_can_transfer
                          owner_normalized
                          permissions
                          properties
                          schema_version
                          sponsorship
                          token_limit
                          token_prefix
                          token_property_permissions
                          tokens_count
                          transfers_count
                          variable_on_chain_schema
                        }
                      }
                    }",
                OperationName = "MyQuery",
                Variables = new GraphQLVariables
                {
                    where = filter,
                    offset = offset,
                    limit = limit,
                },
            };

            var graphQLResponse = await client.SendQueryAsync<ResponseType>(request, token);

            if (graphQLResponse.Errors != null && graphQLResponse.Errors.Length > 0)
            {
                foreach (var error in graphQLResponse.Errors)
                {
                    throw new Exception(error.Message);
                }
            }

            return graphQLResponse.Data.Collections.Data;
        }
    }
}

