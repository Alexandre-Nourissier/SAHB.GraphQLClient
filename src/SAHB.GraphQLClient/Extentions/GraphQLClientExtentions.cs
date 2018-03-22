using System;
using System.Threading.Tasks;
using SAHB.GraphQLClient.Exceptions;
using SAHB.GraphQLClient.QueryGenerator;

namespace SAHB.GraphQLClient.Extentions
{
    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// Contains extention methods for <see cref="IGraphQLClient"/>
    /// </summary>
    public static class GraphQLClientExtentions
    {
        /// <summary>
        /// Sends a query to a GraphQL server using a specified type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">The type to generate the query from</typeparam>
        /// <param name="client">The IGraphQLHttpClient</param>
        /// <param name="arguments">The arguments used in the query which is inserted in the variables</param>
        /// <returns>The data returned from the query</returns>
        /// <exception cref="GraphQLErrorException">Thrown when validation or GraphQL endpoint returns an error</exception>
        public static Task<T> Query<T>(this IGraphQLClient client, params GraphQLQueryArgument[] arguments) where T : class
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            var query = client.CreateQuery<T>(arguments);
            return query.Execute();
        }

        /// <summary>
        /// Sends a mutation to a GraphQL server using a specified type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">The type to generate the mutation from</typeparam>
        /// <param name="client">The IGraphQLHttpClient</param>
        /// <param name="arguments">The arguments used in the query which is inserted in the variables</param>
        /// <returns>The data returned from the query</returns>
        /// <exception cref="GraphQLErrorException">Thrown when validation or GraphQL endpoint returns an error</exception>
        public static Task<T> Mutate<T>(this IGraphQLClient client, params GraphQLQueryArgument[] arguments) where T : class
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            var query = client.CreateMutation<T>(arguments);
            return query.Execute();
        }
    }
}
