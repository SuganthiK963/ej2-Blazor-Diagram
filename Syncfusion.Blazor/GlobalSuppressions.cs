// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "CA1051: Do not declare visible instance fields ", Justification = "Ignore the rules in the namespace of the Blazor Chart component.", Scope = "namespaceanddescendants", Target = "~N:Syncfusion.Blazor.DataVizCommon")]
[assembly: SuppressMessage("Design", "CA1052: Static holder types should be Static or NotInheritable ", Justification = "Ignore the rules in the namespace of the Blazor Chart component.", Scope = "namespaceanddescendants", Target = "~N:Syncfusion.Blazor.DataVizCommon")]
[assembly: SuppressMessage("Design", "CA1054: URI parameters should not be strings ", Justification = "Ignore the rules in the namespace of the Blazor Chart component.", Scope = "namespaceanddescendants", Target = "~N:Syncfusion.Blazor.DataVizCommon")]
[assembly: SuppressMessage("Build", "CA1501: has an object hierarchy '6' levels deep within the defining module. If possible, eliminate base classes within the hierarchy to decrease its hierarchy level below 6", Justification = "Ignore the rules in the namespace of the Blazor dropdowns component", Scope = "namespaceanddescendants", Target = "~N:Syncfusion.Blazor.Charts")]

[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Ignore the rules in the namespace of the Blazor dropdowns component", Scope = "namespaceanddescendants", Target = "~N:Syncfusion.Blazor.DropDowns")]
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Ignore the rules in the namespace of the Blazor inputs component", Scope = "namespaceanddescendants", Target = "~N:Syncfusion.Blazor.Inputs")]
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Ignore the rules in the namespace of the Blazor calendars component", Scope = "namespaceanddescendants", Target = "~N:Syncfusion.Blazor.Calendars")]
[assembly: SuppressMessage("Build", "CA1501: has an object hierarchy '6' levels deep within the defining module. If possible, eliminate base classes within the hierarchy to decrease its hierarchy level below 6", Justification = "Ignore the rules in the namespace of the Blazor dropdowns component", Scope = "namespaceanddescendants", Target = "~N:Syncfusion.Blazor.DropDowns")]

[assembly: SuppressMessage("Design", "CA1054:URI-like parameters should not be strings", Justification = "Ignore the rule for memeber in DatatUtil", Scope = "member", Target = "~M:Syncfusion.Blazor.Data.DataUtil.GetUrl(System.String,System.String,System.String)~System.String")]
[assembly: SuppressMessage("Design", "CA1055:URI-like return values should not be strings", Justification = "Ignore the rule for memeber in DatatUtil", Scope = "member", Target = "~M:Syncfusion.Blazor.Data.DataUtil.GetUrl(System.String,System.String,System.String)~System.String")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Ignore the rule for memeber in SfBaseExtension", Scope = "member", Target = "~M:Syncfusion.Blazor.SfBaseExtension.InvokeMethod``1(Microsoft.JSInterop.IJSRuntime,System.String,System.String,System.String,System.Object[],System.String,System.Nullable{Microsoft.AspNetCore.Components.ElementReference})~System.Threading.Tasks.ValueTask{``0}")]
[assembly: SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "Ignore the rule for memeber in SfBaseExtension", Scope = "member", Target = "~M:Syncfusion.Blazor.Internal.JSInteropAdaptor.Dispose")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Ignore the rule for memeber in BaseComponent", Scope = "member", Target = "~M:Syncfusion.Blazor.BaseComponent.updateDictionary(System.String,System.Object,System.Collections.Generic.Dictionary{System.String,System.Object})")]
