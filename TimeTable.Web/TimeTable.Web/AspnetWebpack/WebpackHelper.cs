using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace SpbuEducation.TimeTable.Web.AspnetWebpack
{
    /// <summary>
    /// Helps to work with webpack's assets
    /// </summary>
    internal class WebpackHelper
    {
        private readonly static HttpClient backchannel;

        private const string UseDevServerKey = "webpack:wds:use";
        private const string DevServerPortKey = "webpack:wds:port";
        private const string DevServerManifestKey = "webpack:wds:manifest";
        private const string ProductionManifestKey = "webpack:prod:manifest";

        /// <summary>
        /// Defines whether to use webpack dev server (wds)
        /// to serve assets
        /// </summary>
        public static bool UseDevServer => string.Equals(
            ConfigurationManager.AppSettings[UseDevServerKey], "true",
            StringComparison.InvariantCultureIgnoreCase
        );

        private static string DevServerPort =>
            ConfigurationManager.AppSettings[DevServerPortKey];

        private static string DevServerManifest =>
            ConfigurationManager.AppSettings[DevServerManifestKey];

        private static string ProductionManifest =>
            ConfigurationManager.AppSettings[ProductionManifestKey];

        static WebpackHelper()
        {
            if (UseDevServer)
            {
                backchannel = new HttpClient();
                backchannel.DefaultRequestHeaders.Clear();
                backchannel.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json")
                );
            }
        }

        /// <summary>
        /// Reads and returns webpack asset's urls
        /// from the asset manifest file
        /// </summary>
        /// <returns></returns>
        public static async Task<WebpackAssetsUrls> GetAssetsUrlsAsync(
            bool continueOnCapturedContext)
        {
            Uri wdsAddress = null;
            Uri assetManifestUri = null;
            Stream assetManifestStream = null;

            Func<string, string> getAssetUrl = (assetPath) => UseDevServer ?
                new Uri(GetUriDirectory(assetManifestUri), assetPath).ToString() :
                VirtualPathUtility.ToAbsolute(
                    VirtualPathUtility.GetDirectory(ProductionManifest) + assetPath
                );

            try
            {
                // Open the stream to manifest resource
                if (UseDevServer)
                {
                    wdsAddress = GetWebpackDevServerAddress();

                    assetManifestUri = new Uri(
                        wdsAddress,
                        VirtualPathUtility.ToAbsolute(DevServerManifest)
                    );

                    try
                    {
                        assetManifestStream = await backchannel
                            .GetStreamAsync(assetManifestUri)
                            .ConfigureAwait(continueOnCapturedContext);
                    }
                    catch (HttpRequestException)
                    {
                        throw new InvalidOperationException(
                            $"Failed to retrieve {assetManifestUri}. " +
                            $"Check whether devpack web server is up and running " +
                            $"on {wdsAddress}"
                        );
                    }
                }
                else
                {
                    var assetManifestFilePath = HttpContext.Current.Server.MapPath(
                        ProductionManifest
                    );

                    assetManifestStream = new FileStream(assetManifestFilePath,
                        FileMode.Open,
                        FileAccess.Read,
                        FileShare.ReadWrite | FileShare.Delete
                    );
                }

                // Read and parse manifest json contents
                // and return it as a dictionary with correct urls
                // dependening on UseWebpackDevServer value
                using (var reader = new StreamReader(assetManifestStream))
                {
                    var manifestJson = await reader
                        .ReadToEndAsync()
                        .ConfigureAwait(continueOnCapturedContext);

                    try
                    {
                        return new WebpackAssetsUrls(JsonConvert
                            .DeserializeObject<Dictionary<string, string>>(manifestJson)
                            .ToDictionary(
                                pair => pair.Key,
                                pair => getAssetUrl(pair.Value)
                            )
                        );
                        
                    }
                    catch (JsonException)
                    {
                        return WebpackAssetsUrls.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                var handle =
                    ex is System.Security.SecurityException ||
                    ex is FileNotFoundException ||
                    ex is DirectoryNotFoundException ||
                    ex is NotSupportedException ||
                    ex is IOException;
                if (handle)
                {
                    return WebpackAssetsUrls.Empty;
                }

                throw ex;
            }
            finally
            {
                assetManifestStream?.Dispose();
            }
        }

        private static Uri GetUriDirectory(Uri uri)
        {
            return new Uri(uri, ".");
        }

        private static Uri GetWebpackDevServerAddress()
        {
            if (DevServerPort == null)
            {
                throw new ConfigurationErrorsException(
                    $"Invalid configuration: {DevServerPortKey} is missing. " +
                    $"Set {UseDevServerKey} to 'false' or provide a valid value " +
                    $"of {DevServerPortKey}"
                );
            }

            var request = HttpContext.Current.Request;
            var devServerRoot = $"{request.Url.Scheme}://{request.Url.Host}:{DevServerPort}";

            return new Uri(devServerRoot);
        }
    }
}