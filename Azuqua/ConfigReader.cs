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
    /// system environment variables or application configuration file
    /// </summary>
    public class ConfigReader
    {
        /// <summary>
        /// Flo's server URL environment variable name
        /// </summary>
        private const string FloServerUrlKeyVariableName = "floServerUrl";

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
        /// Gets Flo Server API URL
        /// </summary>
        public string ServerUrl { get; private set; }

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
            if (File.Exists(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile) 
                && ConfigurationManager.AppSettings.Count > 0
                && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[FloServerUrlKeyVariableName])
                && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[FloAccessKeyVariableName])
                && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[FloAccessSecretVariableName]))
            {
                this.ServerUrl = ConfigurationManager.AppSettings[FloServerUrlKeyVariableName];
                this.Key = ConfigurationManager.AppSettings[FloAccessKeyVariableName];
                this.Secret = ConfigurationManager.AppSettings[FloAccessSecretVariableName];
            }
            else if (!string.IsNullOrEmpty(System.Environment.GetEnvironmentVariable(FloServerUrlKeyVariableName))
                    && !string.IsNullOrEmpty(System.Environment.GetEnvironmentVariable(FloAccessKeyVariableName))
                    && !string.IsNullOrEmpty(System.Environment.GetEnvironmentVariable(FloAccessSecretVariableName)))
            {
                this.ServerUrl = System.Environment.GetEnvironmentVariable(FloServerUrlKeyVariableName);
                this.Key = System.Environment.GetEnvironmentVariable(FloAccessKeyVariableName);
                this.Secret = System.Environment.GetEnvironmentVariable(FloAccessSecretVariableName);
            }
            else
            {
                this.ServerUrl = string.Empty;
                this.Key = string.Empty;
                this.Secret = string.Empty;
            }
        }
    }
}
