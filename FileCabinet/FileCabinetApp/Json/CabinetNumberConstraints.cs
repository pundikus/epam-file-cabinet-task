using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// Class for complex Json deserialization.
    /// Contains cabinet number constraints.
    /// </summary>
    public class CabinetNumberConstraints
    {
        /// <summary>
        /// Gets or sets minimum possible value.
        /// </summary>
        /// <value>Minimum possible value.</value>
        [JsonPropertyName("minValue")]
        public short MinValue { get; set; }

        /// <summary>
        /// Gets or sets maximum possible value.
        /// </summary>
        /// <value>Maximum possible value.</value>
        [JsonPropertyName("maxValue")]
        public short MaxValue { get; set; }
    }
}
