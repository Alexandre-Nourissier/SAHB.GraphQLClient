using System.Threading.Tasks;
using SAHB.GraphQLClient.Result;

namespace SAHB.GraphQLClient.Executor
{
    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// GraphQL executor which executes a query against a http GraphQL server
    /// </summary>
    public interface IGraphQLExecutor
    {
        /// <summary>
        /// Execute the specified GraphQL query
        /// </summary>
        /// <typeparam name="T">The retun type in the data property</typeparam>
        /// <param name="query">The GraphQL query which should be executed</param>
        /// <returns></returns>
        Task<GraphQLDataResult<T>> ExecuteQuery<T>(string query) where T : class;
    }
}