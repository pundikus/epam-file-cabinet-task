using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// Class for complex Json deserialization.
    /// Custom and default rulesets.
    /// </summary>
    public class AppConfiguration
    {
        /// <summary>
        /// Gets or sets default validation ruleset.
        /// </summary>
        /// <value>Default validation ruleset.</value>
        [JsonPropertyName("default")]
        public ValidationRuleset Default { get; set; }

        /// <summary>
        /// Gets or sets custom validation ruleset.
        /// </summary>
        /// <value>Custom validation ruleset.</value>
        [JsonPropertyName("custom")]
        public ValidationRuleset Custom { get; set; }
    }
}
