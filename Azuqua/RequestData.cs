//-----------------------------------------------------------------------
// <copyright file="RequestData.cs" company="Azuqua, Inc">
//     Copyright (c) Azuqua, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Azuqua
{
    using System;
    using System.Security.Cryptography;
    
    using Azuqua;

    /// <summary>
    /// This class encapsulates all necessary data that need to make
    /// a request to Flo's API
    /// </summary>
    public class RequestData
    {
        /// <summary>
        ///  Hash-based Message Authentication Code (HMAC) with SHA256
        /// </summary>
        private string hash = string.Empty;

        /// <summary>
        /// Configuration settings reader
        /// </summary>
        private ConfigReader config = new ConfigReader();

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the RequestData class
        /// </summary>
        /// <param name="data">Data to be sent to Flo API</param>
        public RequestData(string data)
        {
            this.Key = this.config.Key;
            this.Secret = this.config.Secret;
            this.Data = data;
        }

        /// <summary>
        /// Initializes a new instance of the RequestData class
        /// </summary>
        /// <param name="key">Flo API key</param>
        /// <param name="secret">Flo API secret</param>
        /// <param name="data">Data to be sent to Flo API</param>
        public RequestData(string key, string secret, string data)
        {
            this.Key = key;
            this.Secret = secret;
            this.Data = data;
        }

        #endregion

        /// <summary>
        /// Gets or sets Flo API key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets Flo API secret
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// Gets or sets Data to be sent to Flo API
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Gets Hash-based Message Authentication Code (HMAC) with SHA256 of data
        /// </summary>
        public string Hash
        {
            get
            {
                this.GenerateHash();

                return this.hash;
            }
        }

        /// <summary>
        /// Generate Hash-based Message Authentication Code (HMAC) with SHA256 and secret key
        /// </summary>
        private void GenerateHash()
        {
            if (string.IsNullOrEmpty(this.Data))
            {
                this.hash = string.Empty;
                
                return;
            }

            var encoding = new System.Text.UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(this.Secret);
            byte[] messageBytes = encoding.GetBytes(this.Data);

            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashMessage = hmacsha256.ComputeHash(messageBytes);
                var hexString = BitConverter.ToString(hashMessage);
                this.hash = hexString.Replace("-", string.Empty);
            }
        }
    }
}
