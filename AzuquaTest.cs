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
        //const string SECRET = "5686f7797cd31e366608b08fb9460a9926facacd876bb5f70cf872083a34f2cb";
        //const string KEY = "d9da0ea5efb58b22545f909e7754235bb9e7fad5";

        [Test]
        public void CanCreateWithKeyAndSecret()
        {
            string KEY = Environment.GetEnvironmentVariable ("ACCESS_KEY");
            string SECRET = Environment.GetEnvironmentVariable ("ACCESS_SECRET");
            var org = new Org("Azuqua Org", KEY, SECRET);
        }

        [Test]
        public void CanSignData()
        {
            string signed = Azuqua.SignData("data", "verb", "path", "timestamp", "secret");
            string precomputed = "08f14586918357376921c8714eec042b4f1bc64650bed212c2d5ff665dc97515"; 
            Assert.AreEqual(signed, precomputed);
        }

        [Test]
        public void CanCreateOrgWithKeyAndSecret() 
        {
            string KEY = Environment.GetEnvironmentVariable ("ACCESS_KEY");
            string SECRET = Environment.GetEnvironmentVariable ("ACCESS_SECRET");            
            Org org = new Org("Azuqua Org Name", KEY, SECRET);
            foreach (Flo flo in org.GetFlos()) {
                // Call each flo method.
                flo.Enable ();
                var resp = flo.Invoke ("{\"a\":\"Test Data\"}");
                Assert.IsNotNull(resp);
                Assert.AreEqual(resp, "{\"data\":{\"a\":\"Test Data\"}}");
            }
        }

        [Test]
        public void CanInvokeFloWithAlias() 
        {
            string EMAIL = Environment.GetEnvironmentVariable ("AZUQUA_EMAIL");
            string PASSWORD = Environment.GetEnvironmentVariable ("AZUQUA_PASSWORD");

            List<Org> orgs = Azuqua.Login (EMAIL, PASSWORD);
            foreach (Org org in orgs) {
                foreach (Flo flo in org.GetFlos()) {
                    flo.Enable ();
                    string resp = flo.Invoke ("{\"a\":\"Test Data\"}");
                    Assert.IsNotNull(resp);
                    Assert.AreEqual(resp, "{\"data\":{\"a\":\"Test Data\"}}");
                }
            }
        }
    }
}
