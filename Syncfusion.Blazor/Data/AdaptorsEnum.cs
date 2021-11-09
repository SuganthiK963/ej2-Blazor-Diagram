using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Syncfusion.Blazor
{
    /// <summary>
    /// Specifies the data adaptors.
    /// <list type="bullet">
    /// <item>
    /// <term>BlazorAdaptor</term>
    /// <description>Default. BlazorAdaptor is used to process Enumerable data. It contains methods to process the given collection based on the queries.</description>
    /// </item>
    /// <item>
    /// <term>ODataAdaptor</term>
    /// <description>
    /// OData Adaptor provies ability to consume and manipulate data from OData services.
    /// </description>
    /// </item>
    /// <item>
    /// <term>ODataV4Adaptor</term>
    /// <description>
    /// ODatav4 Adaptor provies ability to consume and manipulate data from OData v4 services
    /// </description>
    /// </item>
    /// <item>
    /// <term>WebApiAdaptor</term>
    /// <description>
    /// WebApi Adaptor provies ability to consume and manipulate data from WebApi services.
    /// This adaptor is targeted to interact with Web API created using OData endpoint, it is extended from ODataAdaptor
    /// </description>
    /// </item>
    /// <item>
    /// <term>UrlAdaptor</term>
    /// <description>
    /// URL Adaptor is used when you are required to interact with all kind of remote services to retrieve data.
    /// </description>
    /// </item>
    /// <item>
    /// <term>RemoteSaveAdaptor</term>
    /// <description>
    /// Remote Save Adaptor is used for binding JSON data.
    /// It interacts with remote services only for CRUD operations.
    /// </description>
    /// </item>
    /// <item>
    /// <term>CustomAdaptor</term>
    /// <description>
    /// CustomAdaptor specifies that own data query and manipulation logic has been provided using custom adaptor component
    /// extended from DataAdaptor class.
    /// </description>
    /// </item>
    /// <item>
    /// <term>JsonAdaptor</term>
    /// <description>
    /// JsonAdaptor is used to process JSON data at the client side. It contains methods to process the given JSON data based on the queries.
    /// </description>
    /// </item>
    /// </list>
    /// </summary>
    public enum Adaptors
    {
        /// <summary>
        /// JsonAdaptor is used to process JSON data at the client side. It contains methods to process the given JSON data based on the queries.
        /// </summary>
        [EnumMember(Value = "JsonAdaptor")]
        JsonAdaptor,

        /// <summary>
        /// BlazorAdaptor is used to process Enumerable data. It contains methods to process the given collection based on the queries.
        /// </summary>
        [EnumMember(Value = "BlazorAdaptor")]
        BlazorAdaptor,

        /// <summary>
        /// OData Adaptor provies ability to consume and manipulate data from OData services.
        /// </summary>
        [EnumMember(Value = "ODataAdaptor")]
        ODataAdaptor,

        /// <summary>
        /// OData v4 Adaptor provies ability to consume and manipulate data from OData v4 services.
        /// </summary>
        [EnumMember(Value = "ODataV4Adaptor")]
        ODataV4Adaptor,

        /// <summary>
        /// URL Adaptor is used when you are required to interact with all kind of remote services to retrieve data.
        /// </summary>
        [EnumMember(Value = "UrlAdaptor")]
        UrlAdaptor,

        /// <summary>
        /// WebApi Adaptor provies ability to consume and manipulate data from WebApi services.
        /// This adaptor is targeted to interact with Web API created using OData endpoint, it is extended from ODataAdaptor
        /// </summary>
        [EnumMember(Value = "WebApiAdaptor")]
        WebApiAdaptor,

        /// <summary>
        /// Remote Save Adaptor is used for binding JSON data.
        /// It interacts with remote services only for CRUD operations.
        /// </summary>
        [EnumMember(Value = "RemoteSaveAdaptor")]
        RemoteSaveAdaptor,

        /// <summary>
        /// CustomAdaptor specifies that own data query and manipulation logic has been provided using custom adaptor component
        /// extended from DataAdaptor class.
        /// </summary>
        [EnumMember(Value = "CustomAdaptor")]
        CustomAdaptor,
    }

    /// <summary>
    /// Specifies the operator list used for filtering or searching operations.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Operator
    {
        /// <summary>
        /// No operator is selected.
        /// </summary>
        [EnumMember(Value = "none")]
        None,

        /// <summary>
        /// Filter items by contains operator.
        /// </summary>
        [EnumMember(Value = "contains")]
        Contains,

        /// <summary>
        /// Filter items by starts with operator.
        /// </summary>
        [EnumMember(Value = "startswith")]
        StartsWith,

        /// <summary>
        /// Filter items by ends with operator.
        /// </summary>
        [EnumMember(Value = "endswith")]
        EndsWith,

        /// <summary>
        /// Filter items by equal operator.
        /// </summary>
        [EnumMember(Value = "equal")]
        Equal,

        /// <summary>
        /// Filter items by not-equal operator.
        /// </summary>
        [EnumMember(Value = "notequal")]
        NotEqual,

        /// <summary>
        /// Filter items by greater than operator.
        /// </summary>
        [EnumMember(Value = "greaterthan")]
        GreaterThan,

        /// <summary>
        /// Filter items by greater than or equal operator.
        /// </summary>
        [EnumMember(Value = "greaterthanorequal")]
        GreaterThanOrEqual,

        /// <summary>
        /// Filter items by less than operator.
        /// </summary>
        [EnumMember(Value = "lessthan")]
        LessThan,

        /// <summary>
        /// Filter items by less than or equal operator.
        /// </summary>
        [EnumMember(Value = "lessthanorequal")]
        LessThanOrEqual,
    }
}
