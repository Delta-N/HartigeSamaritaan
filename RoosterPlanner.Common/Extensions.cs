using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoosterPlanner.Common
{
    public static class Extensions
    {
        #region Uri
        /// <summary>
        /// Adds the uriPath to the Uri. uriPath must be relative and without querystring value.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="uriPath"></param>
        /// <returns></returns>
        public static Uri CombinePath(this Uri uri, string uriPath)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");

            if (String.IsNullOrEmpty(uriPath) || !Uri.IsWellFormedUriString(uriPath, UriKind.Relative))
                return uri;

            string pathDelimeter = "/";
            if (uriPath.StartsWith(pathDelimeter))
                uriPath = uriPath.Substring(1);

            if (uri.AbsoluteUri.EndsWith(pathDelimeter))
                pathDelimeter = String.Empty;

            return new Uri($"{uri.AbsoluteUri}{pathDelimeter}{uriPath}");
        }

        public static Uri AddPath(this Uri uri, string uriPath)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");

            if (String.IsNullOrEmpty(uriPath))
                return uri;

            //If uriPath contains querystring, strip and add later
            string qryString = null;
            int qryStringPos = uriPath.IndexOf("?");
            if (qryStringPos != -1)
            {
                qryString = uriPath.Substring(qryStringPos + 1);
                uriPath = uriPath.Substring(0, qryStringPos);
            }
            if (Uri.IsWellFormedUriString(uriPath, UriKind.Relative))
            {
                UriBuilder uriBuilder = new UriBuilder(uri);
                string pathDelimeter = "/";
                if (uriBuilder.Path.EndsWith(pathDelimeter))
                    pathDelimeter = String.Empty;

                uriBuilder.Path = $"{uriBuilder.Path}{pathDelimeter}{uriPath}".Replace("//", "/");

                if (!String.IsNullOrEmpty(qryString))
                    uriBuilder.Query = qryString;
                return uriBuilder.Uri;
            }

            return uri;
        }

        /// <summary>
        /// Adds the path's. uriPath can contain querystring value.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="uriPath"></param>
        /// <returns></returns>
        public static Uri AddPath(this Uri uri, params string[] uriPath)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");

            if (uriPath == null || uriPath.Length == 0)
                return uri;

            //If one of the uriPath contains querystring, strip and add later
            int qryStringPos = 0;

            string pathDelimeter = "/";
            UriBuilder uriBuilder = new UriBuilder(uri);

            for (int i = 0; i < uriPath.Length; i++)
            {
                if (Uri.IsWellFormedUriString(uriPath[i], UriKind.Relative))
                {
                    qryStringPos = uriPath[i].IndexOf("?");
                    if (qryStringPos != -1)
                    {
                        if (String.IsNullOrEmpty(uriBuilder.Query))
                            uriBuilder.Query = $"?{uriPath[i].Substring(qryStringPos + 1)}";
                        else
                            uriBuilder.Query = $"{uriBuilder.Query.Substring(1)}&{uriPath[i].Substring(qryStringPos + 1)}";
                        uriPath[i] = uriPath[i].Substring(0, qryStringPos);
                    }

                    pathDelimeter = "/";
                    if (uriBuilder.Path.EndsWith(pathDelimeter) || uriPath[i].StartsWith(pathDelimeter))
                        pathDelimeter = String.Empty;

                    uriBuilder.Path = $"{uriBuilder.Path}{pathDelimeter}{uriPath[i]}".Replace("//", "/");
                }
            }

            return uriBuilder.Uri;
        }
        #endregion
    }

    public class UriExtensions
    {
        /// <summary>
        /// Combines two Uri strings with a '/' delimeter.
        /// </summary>
        /// <param name="uriPath1">Uri string, can not be Null.</param>
        /// <param name="uriPath2"></param>
        /// <returns></returns>
        public static string CombinePath(string uriPath1, string uriPath2)
        {
            if (string.IsNullOrEmpty(uriPath1))
                throw new ArgumentNullException("uriPath1");

            if (string.IsNullOrEmpty(uriPath2))
                return uriPath1;

            string pathDelimeter = "/";
            if (uriPath2.StartsWith(pathDelimeter))
                uriPath2 = uriPath2.Substring(1);

            if (uriPath1.EndsWith(pathDelimeter))
                pathDelimeter = string.Empty;

            return $"{uriPath1}{pathDelimeter}{uriPath2}";
        }

        /// <summary>
        /// Combines two Uri strings with a '/' delimeter.
        /// </summary>
        /// <param name="uriPath1">Uri string, can not be Null.</param>
        /// <param name="uriPath2"></param>
        /// <returns></returns>
        public static string CombinePath(params string[] uriPath)
        {
            if (uriPath == null)
                throw new ArgumentNullException("uriPath");
            if (uriPath.Length == 0)
                return null;

            string pathDelimeter = "/";
            StringBuilder newUri = new StringBuilder();
            for (int i = 0; i < uriPath.Length; i++)
            {
                if (i != 0 && uriPath[i].StartsWith(pathDelimeter))
                    uriPath[i] = uriPath[i].Substring(1);
                if (newUri.Length != 0 && !newUri.ToString().EndsWith(pathDelimeter))
                    newUri.Append(pathDelimeter);
                newUri.Append(uriPath[i]);
            }
            return newUri.ToString();
        }
    }

    public class JsonSerializerSettingsExtensions
    {
        public static JsonSerializerSettings DefaultSettings()
        {
            DefaultContractResolver contractResolver = new DefaultContractResolver {
                NamingStrategy = new CamelCaseNamingStrategy(false, false)
            };

            return new JsonSerializerSettings {
                ContractResolver = contractResolver,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DefaultValueHandling = DefaultValueHandling.Include,
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }
    }
}
