//-----------------------------------------------------------------------
// <copyright file="Azuqua.cs" company="Azuqua, Inc">
//     Copyright (c) Azuqua, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Security.Cryptography;

/// This class encapsulates all necessary data that need to make
/// a request to Flo's API
public class Azuqua {

    /// TODO(Lito): Configuration settings reader
    private string ROUTES_list_path = "/account/flos";
    private string ROUTES_list_method = "get";
    private string ROUTES_invoke_path = "/flo/:id/invoke";

    private string accessKey, accessSecret;
    /// Constructor
    public Azuqua(string accessKey, string accessSecret) {
        this.accessKey = accessKey;
        this.accessSecret = accessKey;
    }

    public void MakeRequest(string path, string verb, string data) {
        if (data.Length < 1) {
            data = "";
        }
        Dictionary<string, string> headers = HTTP_headers;
        string timestamp = DateTime.UtcNow.ToString("s"); // ISO formatted date

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://dcrypt.it/decrypt/paste"); 
        //TODO: fixme
        string hash = this.SignData(data, verb, path, timestamp);

        if (verb == "get") {

        }
        else if (verb == "put") {

        }
    }

    /// Generate Hash-based Message Authentication Code (HMAC) with 
    /// SHA256 and secret key
    private string SignData(string path, string verb, string data, string timestamp) {
        if (string.IsNullOrEmpty(data)) {
            return "";
        }

        string meta = verb.ToLower()+":"+path+":"+timestamp;

        var encoding = new System.Text.UTF8Encoding();
        byte[] keyByte = encoding.GetBytes(this.accessSecret);
        byte[] messageBytes = encoding.GetBytes(data+meta);

        using (var hmacsha256 = new HMACSHA256(keyByte)) {
            byte[] hashMessage = hmacsha256.ComputeHash(messageBytes);
            var hexString = BitConverter.ToString(hashMessage);
            // By default, BitConverter's output is '-' separated hex pairs
            return hexString.Replace("-", "");
        }
    }
}
