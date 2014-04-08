using System;
using System.Collections.Generic;
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
        #region Have only environment variables

        [Test]
        public void ShouldReturnEmptyKeyAndSecretIfEnvironmentVariablesAreMissing()
        {
            ConfigReader reader = new ConfigReader();

            Assert.IsTrue(String.IsNullOrEmpty(reader.Key));
            Assert.IsTrue(String.IsNullOrEmpty(reader.Secret));
        }

        [Test]
        public void ShouldReturnCorrectKeyAndSecretFromEnvironmentVariables()
        {
            string key = "aaaaa";
            string secret = "bbbbb";

            this.SetEnvironmentVariables(key, secret);

            ConfigReader reader = new ConfigReader();

            Assert.AreEqual(key, reader.Key);
            Assert.AreEqual(secret, reader.Secret);

            this.DeleteEnvironmentVariables();
        }

        #endregion

        #region Have only configuration file

        [Test]
        public void ShouldReturnEmptyKeyAndSecretIfConfigFileIsMissing()
        {
            ConfigReader reader = new ConfigReader();

            Assert.IsTrue(String.IsNullOrEmpty(reader.Key));
            Assert.IsTrue(String.IsNullOrEmpty(reader.Secret));
        }

        [Test]
        [Ignore]
        public void ShouldReturnCorrectKeyAndSecretFromConfigurationFile()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Have both environment variables and configuration file

        [Test]
        [Ignore]
        public void ShouldReturnKeyAndSecretFromConfigurationFileWhenEnvironmentVariablesAreMissing()
        {
            throw new NotImplementedException();
        }

        [Test]
        [Ignore]
        public void ShouldReturnKeyAndSecrentFromEnvironmentVariablesWhenConfigurationFileIsMissing()
        {
            throw new NotImplementedException();
        }

        #endregion

        private void SetEnvironmentVariables(string key, string secret)
        {
            string key_name = "floAccessKey";
            string secret_name = "floAccessSecret";

            Environment.SetEnvironmentVariable(key_name, key);
            Environment.SetEnvironmentVariable(secret_name, secret);
        }
        
        private void DeleteEnvironmentVariables()
        {
            string key_name = "floAccessKey";
            string secret_name = "floAccessSecret";

            Environment.SetEnvironmentVariable(key_name, string.Empty);
            Environment.SetEnvironmentVariable(secret_name, string.Empty);
        }
    }
}
