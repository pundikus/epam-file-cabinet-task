using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// Class for complex Json deserialization.
    /// Contains salary constraints.
    /// </summary>
    public class SalaryConstraints
    {
        /// <summary>
        /// Gets or sets minimum possible value.
        /// </summary>
        /// <value>Minimum possible value.</value>
        [JsonPropertyName("minValue")]
        public decimal MinValue { get; set; }
    }
}
