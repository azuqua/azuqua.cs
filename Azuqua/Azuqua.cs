//-----------------------------------------------------------------------
// <copyright file="Azuqua.cs" company="Azuqua, Inc">
//     Copyright (c) Azuqua, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Security.Cryptography;
using System.Net;
using System.Text;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AzuquaCS
{
    /// <summary>
    /// Org. Represents an Org.
    /// </summary>
    public class Org {
        public string access_key { get; set;}
        public string access_secret { get; set; }
        public string name { get; set;}

        public Org(string name, string access_key, string access_secret) {
            this.name = name;
            this.access_key = access_key;
            this.access_secret = access_secret; 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzuquaCS.Org"/> class with a JSON file. The
        /// JSON file shoud be in the following format: 
        /// 
        /// {
        ///     "name": "Org Name",
        ///     "access_key": "Org key",
        ///     "access_secret": "Org secret"
        /// }
        /// </summary>
        /// <param name="loadFile">Load file.</param>
        public Org(string loadFile) {
            using (StreamReader r = new StreamReader (loadFile)) {
                string json = r.ReadToEnd ();
                Org org = JsonConvert.DeserializeObject<Org> (json);
                this.name = org.name;
                this.access_key = org.access_key;
                this.access_secret = org.access_secret;
            }
        }
            
        /// <summary>
        /// Initializes a new instance of the <see cref="Azuqua.Org"/> class.
        /// </summary>
        public Org() {}

        public List<Flo> GetFlos() {
            string path = "/org/flos";
            string response = Azuqua.MakeRequest(path, "GET", "", this.access_key, this.access_secret);
            List<Flo> flos = JsonConvert.DeserializeObject<List<Flo>> (response);

            // give a reference to this org object to each Flo
            foreach (Flo flo in flos) {
                flo.org = this;
            }

            return flos;
        }
    }

    /// <summary>
    /// Flo. Object represents a Flo. 
    /// </summary>
    public class Flo {
        public string id { get; set; }
        public string alias { get; set; }
        public string name { get; set; }
        public string active { get; set; }
        public string published { get; set; }
        public string description { get; set; }
        public string created { get; set; }
        public string updated { get; set; }

        // give each flo reference to its org
        public Org org { get; set; }

        public Flo(string name, string alias) {
            this.name = name;
            this.alias = alias;
        }

        /// <summary>
        /// Invoke the Flo with the specified data.
        /// </summary>
        /// <param name="data">Data. A string that represents json data.</param>
        public string Invoke(string data) {
            string path = "/flo/"+alias+"/invoke";
            string resp = Request(path, "POST", data);
            return resp;
        }

        /// <summary>
        /// Read this Flo instance. Returns a string that represents a Flo JSON object.
        /// </summary>
        public string Read() {
            string path = "/flo/"+alias+"/read";
            return Request(path, "GET", "");
        }

        /// <summary>
        /// Enable this Flo instance. Returns a string that represents a Flo JSON object.
        /// </summary>
        public string Enable() {
            string path = "/flo/"+alias+"/enable";
            return Request(path, "GET", "");
        }

        /// <summary>
        /// Disable this Flo instance. Returns a string that represents a Flo JSON object.
        /// </summary>
        public string Disable() {
            string path = "/flo/"+alias+"/disable";
            return Request(path, "GET", "");
        }

        /// <summary>
        /// Private method that makes an Azuqua request.
        /// </summary>
        /// <param name="path">Path. Url path.</param>
        /// <param name="method">Method. HTTP method.</param>
        /// <param name="data">Data. Data to send to request.</param>
        private string Request(string path, string method, string data) {
            return Azuqua.MakeRequest(path, method, data, org.access_key, org.access_secret);
        }
    }
        
    /// <summary>
    /// Azuqua. This class encapsulates all necessary data that's needed to make a request to the Flo API.
    /// </summary>
    public static class Azuqua 
    {
        public static List<Org> Login(string email, string password) {
            string path = "/org/login";
            string json = JsonConvert.SerializeObject (new LoginCredentials(email, password));
            string resp = MakeRequest(path, "POST", json, "", "");
            return JsonConvert.DeserializeObject<List<Org>> (resp);
        }

        internal class LoginCredentials {
            public string email { get; set; }
            public string password { get; set; }

            public LoginCredentials(string email, string password) {
                this.email = email;
                this.password = password;
            }
        }

        public static string MakeRequest(string path, string verb, string data, string key, string secret) {
            if (data.Length < 1) {
                data = "";
            }
            //TODO: DOUBLE CHECK TIMESTAMP
            string timestamp = DateTime.UtcNow.ToString("s"); // ISO formatted date
            string hash = SignData(data, verb, path, timestamp, secret);

            verb = verb.ToUpper();
//          string host = "https://api.azuqua.com";
            string host = "http://localhost.azuqua.com:6072";
            string uri = host + path;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = verb;
            request.ContentType = "application/json";
            request.Headers.Add("x-api-accessKey", key);
            request.Headers.Add("x-api-timestamp", timestamp);
            request.Headers.Add("x-api-hash", hash);

            // Make it happen! 
            if (verb == "GET") {
                request.ContentLength = 0;
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                using (var streamReader = new StreamReader(responseStream)) {
                    string responseText = streamReader.ReadToEnd();
                    return responseText;        
                }
            }
            else if (verb == "POST") {
                byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = dataBytes.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(dataBytes, 0, dataBytes.Length);

                // .GetResponse both sends to the server and gets the response
                WebResponse response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                using (var streamReader = new StreamReader(responseStream)) {
                    string responseText = streamReader.ReadToEnd();
                    return responseText;        
                }
            }
            else {
                // Should never get here- requests should be only GET or POST
                throw new Exception("Azuqua only supports GET or POST calls");
            }

        }

        /// Generate Hash-based Message Authentication Code (HMAC) with 
        /// SHA256 and secret key
        public static string SignData(string data, string verb, string path, string timestamp, string secret) {
            if (string.IsNullOrEmpty(data)) {
                data = "";
            }

            string meta = verb.ToLower()+":"+path+":"+timestamp;

            var encoding = new System.Text.UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(meta+data);

            using (var hmacsha256 = new HMACSHA256(keyByte)) {
                byte[] hashMessage = hmacsha256.ComputeHash(messageBytes);
                var hexString = BitConverter.ToString(hashMessage);
                // By default, BitConverter's output is '-' separated hex pairs
                return hexString.Replace("-", "").ToLower();
            }
        }
    }
}
