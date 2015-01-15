using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit;
using NUnit.Framework;

using Azuqua;
using System.Security.Cryptography;
using System.Configuration;

namespace Azuqua.Test
{
    [TestFixture]
    public class RequestDataTest
    {
        private const string ServerUrlName = "floServerUrl";

        [Test]
        public void CanCreateWithKeyAndSecret()
        {
            Azuqua azu = new Azuqua("key", "secret");

            Assert.IsNotNull(azu);
        }

        [Test]
        public void CanCreateWithKeySecretAndData()
        {
            string key = "aaaaa";
            string secret = "bbbbb";
            string data = "";

            private const Azuqua azu = new Azuqua("key", "secret");

            Assert.IsNotNull(data);
        }

        [Test]
        public void CanCreateWithEmptyKeySecret()
        {
            string key = "";
            string secret = "";
            string data = "ccc";

            RequestData requestData = new RequestData(key, secret, data);

            Assert.IsNotNull(data);
        }

        [Test]
        public void ShouldSetAndGetPropertiesCorrectly()
        {
            string key = "aaa";
            string secret = "bbb";
            string data = "ccc";

            RequestData requestData = new RequestData(key, secret, data);

            Assert.AreEqual(key, requestData.Key);
            Assert.AreEqual(secret, requestData.Secret);
            Assert.AreEqual(data, requestData.Data);
        }

        [Test]
        public void ShouldReadKeyAndSecretFromEnvironmentVariables()
        {
            string key = "aaa";
            string secret = "bbb";
            string data = "ccc";

            SetEnvironmentVariables(key, secret);

            RequestData requestData = new RequestData(data);
            string hash = this.CreateHash(secret, data);

            Assert.AreEqual(hash, requestData.Hash);

            DeleteEnvironmentVariables();
        }

        [Test]
        public void ShouldReadKeyAndSecretFromAppSettings()
        {
            string key = "aaa";
            string secret = "bbb";
            string data = "ccc";

            DeleteEnvironmentVariables();
            SetAppSettings(key, secret);

            RequestData requestData = new RequestData(data);
            string hash = this.CreateHash(secret, data);

            Assert.AreEqual(hash, requestData.Hash);

            RemoveAppSettings();
        }

        [Test]
        public void CanReadHashProperty()
        {
            string data = "";

            RequestData requestData = new RequestData(data);

            Assert.IsNotNull(requestData.Hash);
        }

        [Test]
        public void CannotWriteToHashProperty()
        {
            Assert.IsFalse(typeof(RequestData).GetProperty("Hash").CanWrite);
        }

        [Test]
        public void ShouldGenerateHashFromData()
        {
            string secret = "bbbbb";
            string data = "ccccc";
            string hash = string.Empty;
            RequestData requestData = new RequestData(data);
            requestData.Secret = secret;

            hash = this.CreateHash(secret, data);

            Assert.AreEqual(hash, requestData.Hash);
        }

        [Test]
        public void ShouldGenerateEmptyHashIfDataIsNullOrEmpty()
        {
            string data = "";

            RequestData requestData = new RequestData(data);

            Assert.IsTrue(string.IsNullOrEmpty(requestData.Hash));
        }

        private string CreateHash(string secret, string data)
        {
            var encoding = new System.Text.UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(data);
            string hash = string.Empty;

            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashMessage = hmacsha256.ComputeHash(messageBytes);
                var hexString = BitConverter.ToString(hashMessage);
                hash = hexString.Replace("-", "");
            }

            return hash;
        }

        private void SetEnvironmentVariables(string key, string secret)
        {
            Environment.SetEnvironmentVariable(ServerUrlName, "http://test.url");
            Environment.SetEnvironmentVariable(KeyName, key);
            Environment.SetEnvironmentVariable(SecretName, secret);
        }

        private void DeleteEnvironmentVariables()
        {
            Environment.SetEnvironmentVariable(ServerUrlName, string.Empty);
            Environment.SetEnvironmentVariable(KeyName, string.Empty);
            Environment.SetEnvironmentVariable(SecretName, string.Empty);
        }

        private void SetAppSettings(string key, string secret)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (ConfigurationManager.AppSettings.AllKeys.Contains(ServerUrlName))
            {
                config.AppSettings.Settings[ServerUrlName].Value = "http://test.url";
            }
            else
            {
                config.AppSettings.Settings.Add(ServerUrlName, "http://test.url");
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
    }
}
