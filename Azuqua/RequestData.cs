//-----------------------------------------------------------------------
// <copyright file="RequestData.cs" company="Azuqua, Inc">
//     Copyright (c) Azuqua, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Azuqua
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// This class encapsulates all necessary data that need to make
    /// a request to Flo's API
    /// </summary>
    public class RequestData
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the RequestData class
        /// </summary>
        /// <param name="data">Data to be sent to Flo API</param>
        public RequestData(string data) : this(string.Empty, string.Empty, data)
        {
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
    }
}
