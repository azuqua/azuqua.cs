//-----------------------------------------------------------------------
// <copyright file="Azuqua.cs" company="Azuqua, Inc">
//     Copyright (c) Azuqua, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Security.Cryptography;

/// Utility class to contain account info
public class AzuquaAccount {
    public AzuquaAccount(string accessKey, string accessSecret) {
        AccessKey = accessKey;
        AccessSecret = accessKey;
    }
    public string AccessKey {get; set;}
    public string AccessSecret {get; set;}
}

/// This class encapsulates all necessary data that need to make
/// a request to Flo's API
public class Azuqua {

    /// Configuration settings reader
    private ConfigReader config = new ConfigReader();

    private Dictionary<string, string> ROUTES = new Dictionary<string, string>() 
    {
        {"list", new Dictionary<string, string>()
            {
                {"path", "/flo/:id/invoke"},
                {"method", "post"}
            }
        }
    }

    /// Constructor
    public Azuqua(string accessKey, string accessSecret) {
        this.accessKey = accessKey;
        this.accessSecret = accessKey;
    }

    public void MakeRequest() {
        client = HTTP

    }

    /// Generate Hash-based Message Authentication Code (HMAC) with 
    /// SHA256 and secret key
    private string GenerateHash() {
        if (string.IsNullOrEmpty(this.Data))
        {
            this.hash = string.Empty;
            
            return;
        }

        var encoding = new System.Text.UTF8Encoding();
        byte[] keyByte = encoding.GetBytes(this.Secret);
        byte[] messageBytes = encoding.GetBytes(this.Data);

        using (var hmacsha256 = new HMACSHA256(keyByte)) {
            byte[] hashMessage = hmacsha256.ComputeHash(messageBytes);
            var hexString = BitConverter.ToString(hashMessage);
            // TODO
            return hexString.Replace("-", "");
        }
    }
}
