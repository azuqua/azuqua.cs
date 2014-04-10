using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit;
using NUnit.Framework;

using Azuqua;

namespace Azuqua.Test
{
    [TestFixture]
    public class RequestDataTest
    {
        [Test]
        public void CanCreateWithData()
        {
            string data = "";

            RequestData requestData = new RequestData(data);

            Assert.IsNotNull(data);
        }

        [Test]
        public void CanCreateWithKeySecretAndData()
        {
            string key = "aaaaa";
            string secret = "bbbbb";
            string data = "";

            RequestData requestData = new RequestData(key, secret, data);

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
    }
}
