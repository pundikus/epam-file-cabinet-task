using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// Class for complex Json deserialization.
    /// Contains category constraints.
    /// </summary>
    public class CategoryConstraints
    {
        /// <summary>
        /// Gets or sets minimum possible value.
        /// </summary>
        /// <value>Minimum possible value.</value>
        [JsonPropertyName("minValue")]
        public int MinValue { get; set; }

        /// <summary>
        /// Gets or sets maximum possible value.
        /// </summary>
        /// <value>Maximum possible value.</value>
        [JsonPropertyName("maxValue")]
        public int MaxValue { get; set; }
    }
}
