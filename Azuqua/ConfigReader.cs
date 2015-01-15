//-----------------------------------------------------------------------
// <copyright file="ConfigReader.cs" company="Azuqua, Inc">
//     Copyright (c) Azuqua, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Azuqua
{
    using System;
    using System.Configuration;
    using System.IO;

    /// <summary>
    /// This class is responsible for reading Flo Key and Secret from
    /// system environment variables, or just take them on instantiation
    /// </summary>
    public class ConfigReader
    {
        /// <summary>
        /// Flo access key environment var name
        /// </summary>
        private const string FloAccessKeyVariableName = "floAccessKey";

        /// <summary>
        /// Flo access secret environment var name
        /// </summary>
        private const string FloAccessSecretVariableName = "floAccessSecret";

        /// <summary>
        /// Initializes a new instance of the ConfigReader class
        /// </summary>
        public ConfigReader(string Key, string Secret)
        {
			this.Key = Key;
			this.Secret = Secret;
        }
		public ConfigReader()
		{
            this.LoadFromEnvVars();
		}

        /// <summary>
        /// Flo Server API URL
        /// </summary>
        public const string ServerUrl = "api.azuqua.com";

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
        private void LoadFromEnvVars()
        {
            if (!string.IsNullOrEmpty(System.Environment.GetEnvironmentVariable(FloAccessKeyVariableName))
                    && !string.IsNullOrEmpty(System.Environment.GetEnvironmentVariable(FloAccessSecretVariableName)))
            {
                this.Key = System.Environment.GetEnvironmentVariable(FloAccessKeyVariableName);
                this.Secret = System.Environment.GetEnvironmentVariable(FloAccessSecretVariableName);
            }
            else
            {
                throw new Exception("Missing floAccessKey or FloAccessSecret environment variables");
            }
        }
    }
}
