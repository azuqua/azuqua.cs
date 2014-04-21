using System;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit;
using NUnit.Framework;

using Azuqua;

namespace Azuqua.Test
{
    [TestFixture]
    public class ConfigReaderTests
    {
        private const string ServerUrlName = "floServerUrl";
        private const string KeyName = "floAccessKey";
        private const string SecretName = "floAccessSecret";

        #region Have only environment variables

        [Test]
        public void ShouldReturnEmptyValuesIfEnvironmentVariablesAreMissing()
        {
            this.RemoveAppSettings();

            ConfigReader reader = new ConfigReader();

            Assert.IsTrue(string.IsNullOrEmpty(reader.ServerUrl));
            Assert.IsTrue(String.IsNullOrEmpty(reader.Key));
            Assert.IsTrue(String.IsNullOrEmpty(reader.Secret));
        }

        [Test]
        public void ShouldReturnCorrectValuesFromEnvironmentVariables()
        {
            string server = "http://test.url";
            string key = "aaaaa";
            string secret = "bbbbb";

            this.RemoveAppSettings();
            this.SetEnvironmentVariables(server, key, secret);

            ConfigReader reader = new ConfigReader();

            Assert.AreEqual(server, reader.ServerUrl);
            Assert.AreEqual(key, reader.Key);
            Assert.AreEqual(secret, reader.Secret);

            this.DeleteEnvironmentVariables();
        }

        #endregion

        #region Have only configuration file

        [Test]
        public void ShouldReturnEmptyValuesIfConfigSettingsAreMissing()
        {
            this.RemoveAppSettings();
            this.DeleteEnvironmentVariables();

            ConfigReader reader = new ConfigReader();

            Assert.IsTrue(String.IsNullOrEmpty(reader.ServerUrl));
            Assert.IsTrue(String.IsNullOrEmpty(reader.Key));
            Assert.IsTrue(String.IsNullOrEmpty(reader.Secret));
        }

        [Test]
        public void ShouldReturnCorrectValuesFromConfigurationFile()
        {
            string server = "http://test.url";
            string key = "aaaaa";
            string secret = "bbbbb";

            this.DeleteEnvironmentVariables();
            this.SetAppSettings(server, key, secret);

            ConfigReader reader = new ConfigReader();

            Assert.NotNull(ConfigurationManager.AppSettings);

            Assert.AreEqual(server, reader.ServerUrl);
            Assert.AreEqual(key, reader.Key);
            Assert.AreEqual(secret, reader.Secret);

            this.RemoveAppSettings();
        }

        [Test]
        public void ShouldNotUseValuesIfOneOfTheConfigSettingsIsMissingOrEmpty()
        {
            string serverUrl = string.Empty;
            string key = "aaaaa";
            string secret = "";

            this.DeleteEnvironmentVariables();
            this.SetAppSettings(serverUrl, key, secret);

            ConfigReader reader = new ConfigReader();

            Assert.AreEqual(string.Empty, reader.ServerUrl);
            Assert.AreEqual(string.Empty, reader.Key);
            Assert.AreEqual(string.Empty, reader.Secret);

            this.RemoveAppSettings();
        }

        #endregion

        #region Have both environment variables and configuration file

        [Test]
        public void ShouldReturnValuesFromConfigurationFileWhenEnvironmentVariablesAreAlsoExist()
        {
            string server = "http://test.url";
            string key = "aaaaa";
            string secret = "bbbbb";

            string server2 = "http://test2.url";
            string key2 = "ccccc";
            string secret2 = "ddddd";

            this.SetAppSettings(server, key, secret);
            this.SetEnvironmentVariables(server2, key2, secret2);

            ConfigReader reader = new ConfigReader();

            Assert.AreEqual(server, reader.ServerUrl);
            Assert.AreEqual(key, reader.Key);
            Assert.AreEqual(secret, reader.Secret);

            this.DeleteEnvironmentVariables();
            this.RemoveAppSettings();
        }

        #endregion

        #region Private methods

        private void SetEnvironmentVariables(string serverUrl, string key, string secret)
        {
            Environment.SetEnvironmentVariable(ServerUrlName, serverUrl);
            Environment.SetEnvironmentVariable(KeyName, key);
            Environment.SetEnvironmentVariable(SecretName, secret);
        }
        
        private void DeleteEnvironmentVariables()
        {
            Environment.SetEnvironmentVariable(ServerUrlName, string.Empty);
            Environment.SetEnvironmentVariable(KeyName, string.Empty);
            Environment.SetEnvironmentVariable(SecretName, string.Empty);
        }

        private void SetAppSettings(string serverUrl, string key, string secret)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (ConfigurationManager.AppSettings.AllKeys.Contains(ServerUrlName))
            {
                config.AppSettings.Settings[ServerUrlName].Value = serverUrl;
            }
            else
            {
                config.AppSettings.Settings.Add(ServerUrlName, serverUrl);
            }

            if (ConfigurationManager.AppSettings.AllKeys.Contains(KeyName))
            {
                config.AppSettings.Settings[KeyName].Value = key;
            }
            else
            {
                config.AppSettings.Settings.Add(KeyName, key);
            }

            if (ConfigurationManager.AppSettings.AllKeys.Contains(SecretName))
            {
                config.AppSettings.Settings[SecretName].Value = secret;
            }
            else
            {
                config.AppSettings.Settings.Add(SecretName, secret);
            }

            config.Save(ConfigurationSaveMode.Modified, true);

            ConfigurationManager.RefreshSection("appSettings");
        }

        private void RemoveAppSettings()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (ConfigurationManager.AppSettings.AllKeys.Contains(ServerUrlName))
            {
                config.AppSettings.Settings.Remove(ServerUrlName);
            }

            if (ConfigurationManager.AppSettings.AllKeys.Contains(KeyName))
            {
                config.AppSettings.Settings.Remove(KeyName);
            }

            if (ConfigurationManager.AppSettings.AllKeys.Contains(SecretName))
            {
                config.AppSettings.Settings.Remove(SecretName);
            }

            config.Save(ConfigurationSaveMode.Modified, true);

            ConfigurationManager.RefreshSection("appSettings");
        }

        #endregion
    }
}
