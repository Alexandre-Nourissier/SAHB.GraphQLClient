using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SAHB.GraphQLClient.FieldBuilder.Attributes;

namespace SAHB.GraphQLClient.Introspection
{
    // https://github.com/graphql/graphql-js/blob/f59f44a06ab4d433df4b056d150f79885b62243f/src/utilities/introspectionQuery.js
    // https://github.com/graphql/graphiql/blob/master/src/utility/introspectionQueries.js

    public class GraphQLIntrospectionQuery
    {
        [GraphQLFieldName("__schema")]
        public GraphQLIntrospectionSchema Schema { get; set; }
    }

    public class GraphQLIntrospectionSchema
    {
        public GraphQLIntrospectionType QueryType { get; set; }
        public GraphQLIntrospectionType MutationType { get; set; }
        public GraphQLIntrospectionType SubscriptionType { get; set; }
        public IEnumerable<GraphQLIntrospectionFullType> Types { get; set; }
        public IEnumerable<GraphQLIntrospectionDirective> Directives { get; set; }
    }

    public class GraphQLIntrospectionDirective
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<GraphQLIntrospectionInputValue> Args { get; set; }
        public IEnumerable<GraphQLIntrospectionDirectiveLocation> Locations { get; set; }
    }
    
    [JsonConverter(typeof(StringEnumConverter))]
    public enum GraphQLIntrospectionDirectiveLocation
    {
        [EnumMember(Value = "QUERY")]
        Query,
        [EnumMember(Value = "MUTATION")]
        Mutation,
        [EnumMember(Value = "ENUM")]
        Enum,
        [EnumMember(Value = "ENUM_VALUE")]
        EnumValue,
        [EnumMember(Value = "FIELD")]
        Field,
        [EnumMember(Value = "FIELD_DEFINITION")]
        FieldDefinition,
        [EnumMember(Value = "FRAGMENT_DEFINITION")]
        FragmentDefinition,
        [EnumMember(Value = "FRAGMENT_SPREAD")]
        FragmentSpread,
        [EnumMember(Value = "INLINE_FRAGMENT")]
        InlineFragment
    }

    public class GraphQLIntrospectionType
    {
        public string Name { get; set; }
    }

    public class GraphQLIntrospectionFullType : GraphQLIntrospectionType
    {
        public GraphQLTypeKind Kind { get; set; }
        public string Description { get; set; }

        [GraphQLArguments("includeDeprecated", "Boolean", "fieldsIncludeDeprecated")]
        public IEnumerable<GraphQLIntrospectionField> Fields { get; set; }
        public IEnumerable<GraphQLIntrospectionInputValue> InputFields { get; set; }
        public IEnumerable<GraphQLIntrospectionTypeRef> Interfaces { get; set; }

        [GraphQLArguments("includeDeprecated", "Boolean", "enumValuesIncludeDeprecated")]
        public IEnumerable<GraphQLIntrospectionEnumValue> EnumValues { get; set; }

        public IEnumerable<GraphQLIntrospectionTypeRef> PossibleTypes { get; set; }
    }

    public class GraphQLIntrospectionField
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<GraphQLIntrospectionInputValue> Args { get; set; }
        public GraphQLIntrospectionTypeRef Type { get; set; }
        public bool IsDeprecated { get; set; }
        public string DeprecationReason { get; set; }
    }

    public class GraphQLIntrospectionInputValue
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public GraphQLIntrospectionTypeRef Type { get; set; }
        public string DefaultValue { get; set; }
    }

    public class GraphQLIntrospectionTypeRef : GraphQLIntrospectionTypeRef<GraphQLIntrospectionTypeRef<
        GraphQLIntrospectionTypeRef<GraphQLIntrospectionTypeRef<GraphQLIntrospectionTypeRef<
            GraphQLIntrospectionTypeRef<GraphQLIntrospectionTypeRef<GraphQLIntrospectionOfType>>>>>>>
    {
        
    }

    public class GraphQLIntrospectionTypeRef<T>
    {
        public GraphQLTypeKind Kind { get; set; }
        public string Name { get; set; }
        public T OfType { get; set; }
    }

    public class GraphQLIntrospectionOfType
    {
        public GraphQLTypeKind Kind { get; set; }
        public string Name { get; set; }
    }

    public class GraphQLIntrospectionEnumValue
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDeprecated { get; set; }
        public string DeprecationReason { get; set; }
    }

    // http://facebook.github.io/graphql/October2016/#sec-Type-Kinds
    [JsonConverter(typeof(StringEnumConverter))]
    public enum GraphQLTypeKind
    {
        [EnumMember(Value = "SCALAR")]
        Scalar,
        [EnumMember(Value = "OBJECT")]
        Object,
        [EnumMember(Value = "UNION")]
        Union,
        [EnumMember(Value = "INTERFACE")]
        Interface,
        [EnumMember(Value = "ENUM")]
        Enum,
        [EnumMember(Value = "INPUT_OBJECT")]
        InputObject,
        [EnumMember(Value = "LIST")]
        List,
        [EnumMember(Value = "NON_NULL")]
        NonNull
    }
}
