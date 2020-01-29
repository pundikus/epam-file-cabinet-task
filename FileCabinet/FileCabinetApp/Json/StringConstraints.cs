using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// Class for complex Json deserialization.
    /// String constraints.
    /// </summary>
    public class StringConstraints
    {
        /// <summary>
        /// Gets or sets minimum length of the name.
        /// </summary>
        /// <value>Minimum length of the name.</value>
        [JsonPropertyName("minLength")]
        public int MinLength { get; set; }

        /// <summary>
        /// Gets or sets maximum length of the name.
        /// </summary>
        /// <value>Maximum length of the name.</value>
        [JsonPropertyName("maxLength")]
        public int MaxLength { get; set; }
    }
}
