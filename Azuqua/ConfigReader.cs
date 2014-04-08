//-----------------------------------------------------------------------
// <copyright file="ConfigReader.cs" company="Azuqua, Inc">
//     Copyright (c) Azuqua, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Azuqua
{
    using System;

    /// <summary>
    /// This class is responsible for reading Flo Key and Secret from
    /// system environment variables or application configuration file
    /// </summary>
    public class ConfigReader
    {
        /// <summary>
        /// Flo's access key environment variable name
        /// </summary>
        private const string FloAccessKeyVariableName = "floAccessKey";

        /// <summary>
        /// Flo's access secret environment variable name
        /// </summary>
        private const string FloAccessSecretVariableName = "floAccessSecret";

        /// <summary>
        /// Initializes a new instance of the ConfigReader class
        /// </summary>
        public ConfigReader()
        {
            this.LoadConfigurationData();
        }

        /// <summary>
        /// Gets Flo API key
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// Gets Flo API secret
        /// </summary>
        public string Secret { get; private set; }

        /// <summary>
        /// Load Flo configuration data
        /// </summary>
        private void LoadConfigurationData()
        {
            this.Key = System.Environment.GetEnvironmentVariable(FloAccessKeyVariableName);
            this.Secret = System.Environment.GetEnvironmentVariable(FloAccessSecretVariableName);
        }
    }
}
