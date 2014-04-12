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
        private const string KeyName = "floAccessKey";
        private const string SecretName = "floAccessSecret";

        #region Have only environment variables

        [Test]
        public void ShouldReturnEmptyKeyAndSecretIfEnvironmentVariablesAreMissing()
        {
            this.RemoveAppSettings();

            ConfigReader reader = new ConfigReader();

            Assert.IsTrue(String.IsNullOrEmpty(reader.Key));
            Assert.IsTrue(String.IsNullOrEmpty(reader.Secret));
        }

        [Test]
        public void ShouldReturnCorrectKeyAndSecretFromEnvironmentVariables()
        {
            string key = "aaaaa";
            string secret = "bbbbb";

            this.RemoveAppSettings();
            this.SetEnvironmentVariables(key, secret);

            ConfigReader reader = new ConfigReader();

            Assert.AreEqual(key, reader.Key);
            Assert.AreEqual(secret, reader.Secret);

            this.DeleteEnvironmentVariables();
        }

        #endregion

        #region Have only configuration file

        [Test]
        public void ShouldReturnEmptyKeyAndSecretIfConfigSettingsAreMissing()
        {
            this.RemoveAppSettings();
            this.DeleteEnvironmentVariables();

            ConfigReader reader = new ConfigReader();

            Assert.IsTrue(String.IsNullOrEmpty(reader.Key));
            Assert.IsTrue(String.IsNullOrEmpty(reader.Secret));
        }

        [Test]
        public void ShouldReturnCorrectKeyAndSecretFromConfigurationFile()
        {
            string key = "aaaaa";
            string secret = "bbbbb";

            this.DeleteEnvironmentVariables();
            this.SetAppSettings(key, secret);

            ConfigReader reader = new ConfigReader();

            Assert.NotNull(ConfigurationManager.AppSettings);

            Assert.AreEqual(key, reader.Key);
            Assert.AreEqual(secret, reader.Secret);

            this.RemoveAppSettings();
        }

        [Test]
        public void ShouldNotUseKeyAndSecretIfOneOfTheConfigSettingsIsMissingOrEmpty()
        {
            string key = "aaaaa";
            string secret = "";

            this.DeleteEnvironmentVariables();
            this.SetAppSettings(key, secret);

            ConfigReader reader = new ConfigReader();

            Assert.AreEqual(string.Empty, reader.Key);
            Assert.AreEqual(string.Empty, reader.Secret);

            this.RemoveAppSettings();
        }

        #endregion

        #region Have both environment variables and configuration file

        [Test]
        public void ShouldReturnKeyAndSecretFromConfigurationFileWhenEnvironmentVariablesAreAlsoExist()
        {
            string key = "aaaaa";
            string secret = "bbbbb";
            string key2 = "ccccc";
            string secret2 = "ddddd";

            this.SetAppSettings(key, secret);
            this.SetEnvironmentVariables(key2, secret2);

            ConfigReader reader = new ConfigReader();

            Assert.AreEqual(key, reader.Key);
            Assert.AreEqual(secret, reader.Secret);

            this.DeleteEnvironmentVariables();
            this.RemoveAppSettings();
        }

        #endregion

        #region Private methods

        private void SetEnvironmentVariables(string key, string secret)
        {
            Environment.SetEnvironmentVariable(KeyName, key);
            Environment.SetEnvironmentVariable(SecretName, secret);
        }
        
        private void DeleteEnvironmentVariables()
        {
            Environment.SetEnvironmentVariable(KeyName, string.Empty);
            Environment.SetEnvironmentVariable(SecretName, string.Empty);
        }

        private void SetAppSettings(string key, string secret)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

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
