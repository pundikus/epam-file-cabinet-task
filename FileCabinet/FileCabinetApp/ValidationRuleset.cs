using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// Class for complex Json deserialization. Validation ruleset records properties.
    /// </summary>
    public class ValidationRuleset
    {
        /// <summary>
        /// Gets or sets first name validation ruleset.
        /// </summary>
        /// <value>First name validation ruleset.</value>
        [JsonPropertyName("firstName")]
        public StringConstraints FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name validation ruleset.
        /// </summary>
        /// <value>Last name validation ruleset.</value>
        [JsonPropertyName("lastName")]
        public StringConstraints LastName { get; set; }

        /// <summary>
        /// Gets or sets date of birth validation ruleset.
        /// </summary>
        /// <value>Date of birth validation ruleset.</value>
        [JsonPropertyName("dateOfBirth")]
        public DateOfBirthConstraints DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets cabinet number validation ruleset.
        /// </summary>
        /// <value>Cabinet number validation ruleset.</value>
        [JsonPropertyName("favouriteNumber")]
        public CabinetNumberConstraints CabinetNumber { get; set; }

        /// <summary>
        /// Gets or sets category validation ruleset.
        /// </summary>
        /// <value>Category validation ruleset.</value>
        [JsonPropertyName("favouriteCharacter")]
        public CategoryConstraints Category { get; set; }

        /// <summary>
        /// Gets or sets salary validation ruleset.
        /// </summary>
        /// <value>Salary validation ruleset.</value>
        [JsonPropertyName("donations")]
        public SalaryConstraints Salary { get; set; }
    }
}
