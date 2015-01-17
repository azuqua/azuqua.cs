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
            string precomputed = "c59d6859a39323a4c96facbffe6ba217defd1674825ac713bad611c9ca23e15e"; 

            Assert.Equals(signed, precomputed);
        }

        [Test]
        public void CanInvokeFlo() 
        {
            Azuqua azu = new Azuqua(KEY, SECRET);
            string r = azu.InvokeFlo("httptohttp", "hello world");
            Assert.NotNull(r);
        }

        [Test]
        public void CanInvokeFloWithAlias() 
        {
            Azuqua azu = new Azuqua(KEY, SECRET);
            string r = azu.InvokeFlo("3f8ca2b96024cae4cdacf652b6a322");
            Assert.NotNull(r);
        }
    }
}
