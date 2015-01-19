using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit;
using NUnit.Framework;

using Azuqua = AzuquaCS.Azuqua;
using System.Security.Cryptography;
using System.Configuration;

namespace AzuquaCS.Test
{
    [TestFixture]
    public class AzuquaTest
    {
        const string SECRET = "5686f7797cd31e366608b08fb9460a9926facacd876bb5f70cf872083a34f2cb";
        const string KEY = "d9da0ea5efb58b22545f909e7754235bb9e7fad5";

        [Test]
        public void CanCreateWithKeyAndSecret()
        {
            var azu = new Azuqua("key", "secret");
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void FailIfNoEnvVarsAndNoKeys()
        {
            Environment.SetEnvironmentVariable("floAccessKey", "");
            Environment.SetEnvironmentVariable("floAccessSecret", "");
            Azuqua azu = new Azuqua(); // Try without env vars set
        }

        [Test]
        public void CanCreateWithEnvVars()
        {
            Environment.SetEnvironmentVariable("floAccessKey", KEY);
            Environment.SetEnvironmentVariable("floAccessSecret", SECRET);
            Azuqua azu = new Azuqua(); // Should be ok this time!
        }

        [Test]
        public void CanSignData()
        {
            Azuqua azu = new Azuqua("key", "secret");
            string signed = azu.SignData("data", "verb", "path", "timestamp");
            string precomputed = "08f14586918357376921c8714eec042b4f1bc64650bed212c2d5ff665dc97515"; 

            Assert.AreEqual(signed, precomputed);
        }

        [Test]
        [ExpectedException(typeof(System.Net.WebException))]
        public void CanHitFloEndpoint() 
        {
            Azuqua azu = new Azuqua(KEY, SECRET);
            string r = azu.InvokeFlo("invalidalias", "hello world");
            Console.WriteLine(r);
            Assert.IsNotNull(r);
        }

        [Test]
        public void CanInvokeFloWithAlias() 
        {
            Azuqua azu = new Azuqua("", "");
            string r = azu.InvokeFlo("02d863f0299073f3e1c461874a981e", "hello world");
            Assert.IsNotNull(r);
            Assert.AreEqual(r, "{\"data\":{}}");
        }
    }
}
