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
    public class AzuquaTest
    {
        const string SECRET = "5686f7797cd31e366608b08fb9460a9926facacd876bb5f70cf872083a34f2cb";
        const string KEY = "d9da0ea5efb58b22545f909e7754235bb9e7fad5";

        [Test]
        public void SurfaceAPI()
        {
        }

        [Test]
        public void CanCreateWithKeyAndSecret()
        {
            string key = "aaaaa";
            string secret = "bbbbb";

            const Azuqua azu = new Azuqua("key", "secret");
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void FailIfNoEnvVarsAndNoKeys()
        {
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
            Azuqua azu = new Azuqua("aaa", "bbb");
            string signed = azu.SignData("path", "GET", "hello world", "timestamp");

            string precomputed =  // TODO: verify this in azuqua JS

            AssertEqual(signed, precomputed);

        }
    }
}
