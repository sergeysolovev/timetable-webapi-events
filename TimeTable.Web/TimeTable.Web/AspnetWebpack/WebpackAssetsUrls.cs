using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpbuEducation.TimeTable.Web.AspnetWebpack
{
    /// <summary>
    /// Dictionary, containing assets names and
    /// their urls
    /// </summary>
    public class WebpackAssetsUrls : Dictionary<string, string>
    {
        public WebpackAssetsUrls() : base() { }

        public WebpackAssetsUrls(IDictionary<string, string> source)
            : base(source, EqualityComparer<string>.Default)
        {
        }

        public static WebpackAssetsUrls Empty => new WebpackAssetsUrls();

        /// <summary>
        /// Gets an asset url by its name
        /// or empty string if the name doesn't exist
        /// in the collection
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public string Get(string assetName)
        {
            string assetUrl;
            TryGetValue(assetName, out assetUrl);
            return assetUrl ?? string.Empty;
        }
    }
}