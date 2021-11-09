using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// Defines a keyboard combination that can be used to invoke a command.
    /// </summary>
    public class KeyGesture
    {
        /// <summary>
        /// Gets the key associated with this KeyGesture. The default value is None.
        /// </summary>
        [JsonPropertyName("key")]
        public Keys Key { get; set; } = Keys.None;
        /// <summary>
        /// Gets the modifier keys associated with this KeyGesture. The default value is None.
        /// </summary>
        [JsonPropertyName("modifiers")]
        public ModifierKeys Modifiers { get; set; } = ModifierKeys.None;
    }
    /// <summary>
    /// Specifies a command and a key gesture to define when the command should be executed.
    /// </summary>
    /// <example>
    /// <remarks>
    /// The below code example illustrates how to call a group command. 
    /// </remarks>
    /// <code>
    /// <![CDATA[
    /// private void OnGroup()
    /// {
    ///     diagram.Group();
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public class KeyboardCommand : DiagramObject
    {
        /// <summary>
        /// Specifies the name of the command.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        ///  Specifies a combination of keys and key modifiers, on the recognition of which the command should be executed.
        /// </summary>
        [JsonPropertyName("gesture")]
        public KeyGesture Gesture { get; set; } = new KeyGesture() { };
        /// <summary>
        /// Specifies all additional parameters that are required at runtime.
        /// </summary>
        [JsonPropertyName("parameter")]
        public string Parameter { get; set; } = string.Empty;

        internal override void Dispose()
        {
            Parameter = null;
            Name = null;
            Gesture = null;
        }
    }
}
