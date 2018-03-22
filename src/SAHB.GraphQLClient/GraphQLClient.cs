using System;
using Microsoft.Extensions.Logging;
using SAHB.GraphQLClient.Batching;
using SAHB.GraphQLClient.Batching.Internal;
using SAHB.GraphQLClient.Builder;
using SAHB.GraphQLClient.Builder.Internal;
using SAHB.GraphQLClient.Executor;
using SAHB.GraphQLClient.FieldBuilder;
using SAHB.GraphQLClient.Internal;
using SAHB.GraphQLClient.QueryGenerator;

namespace SAHB.GraphQLClient
{
    // ReSharper disable once InconsistentNaming
    /// <inheritdoc />
    public class GraphQLClient : IGraphQLClient
    {
        private IGraphQLExecutor _executor;
        private readonly IGraphQLFieldBuilder _fieldBuilder;
        private readonly IGraphQLQueryGeneratorFromFields _queryGenerator;
        private ILoggerFactory _loggerFactory;

        /// <summary>
        /// Contains a logger factory for the GraphQLHttpClient
        /// </summary>
        public ILoggerFactory LoggerFactory
        {
            internal get { return _loggerFactory; }
            set
            {
                _loggerFactory = value;
                if (_loggerFactory != null)
                {
                    Logger = _loggerFactory.CreateLogger<GraphQLClient>();
                }
            }
        }

        /// <summary>
        /// Contains the logger for the class
        /// </summary>
        private ILogger<GraphQLClient> Logger { get; set; }

        /// <summary>
        /// Gets or sets the current <see cref="IGraphQLExecutor"/> for the client
        /// </summary>
        public IGraphQLExecutor Executor
        {
            get => _executor;
            set => _executor = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Initilizes a new instance of GraphQL client which supports generating GraphQL queries and mutations from a <see cref="Type"/>
        /// </summary>
        /// <param name="executor">The <see cref="IGraphQLExecutor"/> to use for the GraphQL client</param>
        /// <param name="fieldBuilder">The <see cref="IGraphQLFieldBuilder"/> used for generating the fields used for generating the query</param>
        /// <param name="queryGenerator">The <see cref="IGraphQLQueryGeneratorFromFields"/> used for the GraphQL client</param>
        public GraphQLClient(IGraphQLExecutor executor, IGraphQLFieldBuilder fieldBuilder, IGraphQLQueryGeneratorFromFields queryGenerator)
        {
            Executor = executor ?? throw new ArgumentNullException(nameof(executor));
            _fieldBuilder = fieldBuilder ?? throw new ArgumentNullException(nameof(fieldBuilder));
            _queryGenerator = queryGenerator ?? throw new ArgumentNullException(nameof(queryGenerator));
        }

        /// <summary>
        /// Initilizes a new instance of GraphQL client which supports generating GraphQL queries and mutations from a <see cref="Type"/> using the default <see cref="IGraphQLExecutor"/> and the default <see cref="IGraphQLQueryGeneratorFromFields"/>
        /// </summary>
        /// <returns>A new instance of the GraphQL client</returns>
        public static IGraphQLClient Default()
        {
            return new GraphQLClient(new GraphQLExecutor(), new GraphQLFieldBuilder(), new GraphQLQueryGeneratorFromFields());
        }


        public IGraphQLQuery CreateQuery(Action<IGraphQLBuilder> builder, params GraphQLQueryArgument[] arguments)
        {
            var build = new GraphQLBuilder();
            builder(build);

            // Get the fields and query
            var fields = build.GetFields();
            var query = _queryGenerator.GetQuery(fields, arguments);

            // Get query
            return new GraphQLQuery(query, _executor);
        }

        public IGraphQLQuery<T> CreateQuery<T>(params GraphQLQueryArgument[] arguments) where T : class
        {
            // Get the fields and query
            var query = _queryGenerator.GetQuery(_fieldBuilder.GetFields(typeof(T)), arguments);

            // Get query
            return new GraphQLQuery<T>(query, _executor);
        }

        public IGraphQLQuery CreateMutation(Action<IGraphQLBuilder> builder, params GraphQLQueryArgument[] arguments)
        {
            var build = new GraphQLBuilder();
            builder(build);

            // Get the fields and query
            var fields = build.GetFields();
            var query = _queryGenerator.GetMutation(fields, arguments);

            // Get query
            return new GraphQLQuery(query, _executor);
        }

        public IGraphQLQuery<T> CreateMutation<T>(params GraphQLQueryArgument[] arguments) where T : class
        {
            // Get the fields and query
            var query = _queryGenerator.GetMutation(_fieldBuilder.GetFields(typeof(T)), arguments);

            // Get query
            return new GraphQLQuery<T>(query, _executor);
        }

        public IGraphQLBatch CreateBatch()
        {
            return new GraphQLBatch(_executor, _fieldBuilder, _queryGenerator);
        }
    }
}