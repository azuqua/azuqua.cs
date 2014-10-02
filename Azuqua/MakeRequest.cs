//-----------------------------------------------------------------------
// <copyright file="Azuqua.cs" company="Azuqua, Inc">
//     Copyright (c) Azuqua, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Security.Cryptography;

/// This class encapsulates all necessary data that need to make
/// a request to Flo's API
public class MainTest {
    // TODO: TESTING ONLY (Lito)
    static void Main(string[] args) {
            // Display the number of command line arguments:
            Azuqua azu = new Azuqua("key", "secret");
            string timestamp = DateTime.UtcNow.ToString("s"); // ISO formatted date
            string hash = azu.SignData("/account/flos", "get", "Hello World", timestamp);
            System.Console.WriteLine(hash);
        }
}
// ???
// How the heck do we match the time so exactly to unhash?


public class Azuqua {

    private string accessKey, accessSecret;
    /// Constructor
    public Azuqua(string accessKey, string accessSecret) {
        this.accessKey = accessKey;
        this.accessSecret = accessKey;
    }

    public void MakeRequest(string path, string verb, string data) {
        verb = verb.ToUpper();
        if (data.Length < 1) {
            data = "";
        }
        //Dictionary<string, string> headers = HTTP_headers;
        string timestamp = DateTime.UtcNow.ToString("s"); // ISO formatted date

        byte[] hash = this.SignData(path, verb, data, timestamp);

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(""); 
        request.Method = verb;
        request.ContentLength = hash.Length;
        request.ContentType = ; //TODO
        // Let's take a second to appricate how cool the idea of streams is
        Stream requestStream = request.GetRequestStream();
        dataStream.Write(hash, 0, hash.Length);

        // Make it happen!
        // TODO: In the ifs
        HttpWebRequest response = request.GetResponse();

        // string jsonBody = "{\"x-api-accessKey\": 

        host = "https://api.azuqua.com"
        if (verb == "GET") {
            string address = "/account/flos";

        }
        else if (verb == "PUT") {

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
