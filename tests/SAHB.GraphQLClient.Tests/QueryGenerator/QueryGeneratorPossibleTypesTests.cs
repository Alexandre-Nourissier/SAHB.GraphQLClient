﻿using System;
using System.Collections.Generic;
using System.Text;
using SAHB.GraphQLClient.Extentions;
using SAHB.GraphQLClient.FieldBuilder;
using SAHB.GraphQLClient.QueryGenerator;
using SAHB.GraphQLClient.Tests.Mocks;
using Xunit;

namespace SAHB.GraphQL.Client.Tests.QueryGenerator
{
    public class QueryGeneratorPossibleTypesTests
    {
        [Fact]
        public void Test_Single_Other_Possible_Type_With_Same_Fields()
        {
            var fields = new[]
            {
                new GraphQLField("alias", "field", null, null, new []{new GraphQLPossibleType(new List<GraphQLField> { new GraphQLField("alias", "field", null, null, null) }, "interfaceConcreteType" )}),
            };
            var fieldBuilder = new FieldBuilderMock(fields);
            var queryGenerator = new GraphQLQueryGeneratorFromFields();
            var expected = "{\"query\":\"query{alias:field ... on interfaceConcreteType{alias:field}}\"}";

            var actual = queryGenerator.GetQuery<string>(fieldBuilder); // Typeparameter is ignored since it just returns the fields

            Assert.Equal(expected, actual);
        }
    }
}