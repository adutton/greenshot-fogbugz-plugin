/*
 * Greenshot - a free and open source screenshot tool
 * Copyright (C) 2007-2012  Thomas Braun, Jens Klingen, Robin Krom
 * 
 * For more information see: http://getgreenshot.org/
 * The Greenshot project is hosted on Sourceforge: http://sourceforge.net/projects/greenshot/
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 1 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

using GreenshotPlugin.Core;
using Greenshot.IniFile;

namespace GreenshotFogBugzPlugin {
	/// <summary>
	/// Description of FogBugzUtils.
	/// </summary>
	public class FogBugzUtils {
		private static readonly log4net.ILog LOG = log4net.LogManager.GetLogger(typeof(FogBugzUtils));
		private const string FogBugz_ANONYMOUS_API_KEY = "8116a978913f3cf5dfc8e1117a055056";
		private static FogBugzConfiguration config = IniConfig.GetIniSection<FogBugzConfiguration>();

		private FogBugzUtils() {
		}

		public static void LoadHistory() {
			/*if (config.runtimeFogBugzHistory.Count == config.FogBugzUploadHistory.Count) {
				return;
			}
			// Load the ImUr history
			List<string> hashes = new List<string>();
			foreach(string hash in config.FogBugzUploadHistory.Keys) {
				hashes.Add(hash);
			}
			
			bool saveNeeded = false;

			foreach(string hash in hashes) {
				if (config.runtimeFogBugzHistory.ContainsKey(hash)) {
					// Already loaded
					continue;
			    }
				try {
					FogBugzInfo FogBugzInfo = FogBugzUtils.RetrieveFogBugzInfo(hash, config.FogBugzUploadHistory[hash]);
					if (FogBugzInfo != null) {
						FogBugzUtils.RetrieveFogBugzThumbnail(FogBugzInfo);
						config.runtimeFogBugzHistory.Add(hash, FogBugzInfo);
					} else {
						LOG.DebugFormat("Deleting not found FogBugz {0} from config.", hash);
						config.FogBugzUploadHistory.Remove(hash);
						saveNeeded = true;
					}
				} catch (Exception e) {
					LOG.Error("Problem loading FogBugz history for hash " + hash, e);
				}
			}
			if (saveNeeded) {
				// Save needed changes
				IniConfig.Save();
			}*/
		}

		/// <summary>
		/// Do the actual upload to FogBugz
		/// For more details on the available parameters, see: http://api.FogBugz.com/resources_anon
		/// </summary>
		/// <param name="imageData">byte[] with image data</param>
		/// <returns>FogBugzResponse</returns>
		public static FogBugzInfo UploadToFogBugz(byte[] imageData, int dataLength, string title, string filename) {
			/*StringBuilder uploadRequest = new StringBuilder();
			// Add image
			uploadRequest.Append("image=");
			uploadRequest.Append(HttpUtility.UrlEncode(System.Convert.ToBase64String(imageData, 0, dataLength)));
			// add key
			uploadRequest.Append("&");
			uploadRequest.Append("key=");
			uploadRequest.Append(FogBugz_ANONYMOUS_API_KEY);
			// add title
			if (title != null) {
				uploadRequest.Append("&");
				uploadRequest.Append("title=");
				uploadRequest.Append(HttpUtility.UrlEncode(title, Encoding.UTF8));
			}
			// add filename
			if (filename != null) {
				uploadRequest.Append("&");
				uploadRequest.Append("name=");
				uploadRequest.Append(HttpUtility.UrlEncode(filename, Encoding.UTF8));
			}
			string url = config.FogBugzApiUrl + "/upload";
			HttpWebRequest webRequest = (HttpWebRequest)NetworkHelper.CreatedWebRequest(url);

			webRequest.Method = "POST";
			webRequest.ContentType = "application/x-www-form-urlencoded";
			webRequest.ServicePoint.Expect100Continue = false;
			
			using(StreamWriter streamWriter = new StreamWriter(webRequest.GetRequestStream())) {
				streamWriter.Write(uploadRequest.ToString());
			}
			string responseString;
			using (WebResponse response = webRequest.GetResponse()) {
				Stream responseStream = response.GetResponseStream();
				StreamReader responseReader = new StreamReader(responseStream);
				responseString = responseReader.ReadToEnd();
			}
			LOG.Info(responseString);
			FogBugzInfo FogBugzInfo = FogBugzInfo.ParseResponse(responseString);
			return FogBugzInfo;*/
			return new FogBugzInfo();
        }

		public static void RetrieveFogBugzThumbnail(FogBugzInfo FogBugzInfo) {
			LOG.InfoFormat("Retrieving FogBugz image for {0} with url {1}", FogBugzInfo.Hash, FogBugzInfo);
			HttpWebRequest webRequest = (HttpWebRequest)NetworkHelper.CreatedWebRequest(FogBugzInfo.SmallSquare);
			webRequest.Method = "GET";
			webRequest.ServicePoint.Expect100Continue = false;

			using (WebResponse response = webRequest.GetResponse()) {
				Stream responseStream = response.GetResponseStream();
				FogBugzInfo.Image = Image.FromStream(responseStream);
			}
			return;
		}

		public static FogBugzInfo RetrieveFogBugzInfo(string hash, string deleteHash) {
			/*string url = config.FogBugzApiUrl + "/image/" + hash;
			LOG.InfoFormat("Retrieving FogBugz info for {0} with url {1}", hash, url);
			HttpWebRequest webRequest = (HttpWebRequest)NetworkHelper.CreatedWebRequest(url);
			webRequest.Method = "GET";
			webRequest.ServicePoint.Expect100Continue = false;
			string responseString;
			try {
				using (WebResponse response = webRequest.GetResponse()) {
					Stream responseStream = response.GetResponseStream();
					StreamReader responseReader = new StreamReader(responseStream);
					responseString = responseReader.ReadToEnd();
				}
			} catch (WebException wE) {
				if (wE.Status == WebExceptionStatus.ProtocolError) {
					if (((HttpWebResponse)wE.Response).StatusCode == HttpStatusCode.NotFound) {
						return null;
					}
				}
				throw wE;
			}
			LOG.Info(responseString);
			FogBugzInfo FogBugzInfo = FogBugzInfo.ParseResponse(responseString);
			FogBugzInfo.DeleteHash = deleteHash;
			return FogBugzInfo;*/
			return new FogBugzInfo();
		}

		public static void DeleteFogBugzImage(FogBugzInfo FogBugzInfo) {
			LOG.InfoFormat("Deleting FogBugz image for {0}", FogBugzInfo.DeleteHash);
			
			try {
				string url = config.FogBugzApiUrl + "/delete/" + FogBugzInfo.DeleteHash;
				HttpWebRequest webRequest = (HttpWebRequest)NetworkHelper.CreatedWebRequest(url);
	
				//webRequest.Method = "DELETE";
				webRequest.Method = "GET";
				webRequest.ServicePoint.Expect100Continue = false;
	
				string responseString;
				using (WebResponse response = webRequest.GetResponse()) {
					Stream responseStream = response.GetResponseStream();
					StreamReader responseReader = new StreamReader(responseStream);
					responseString = responseReader.ReadToEnd();
				}
				LOG.InfoFormat("Delete result: {0}", responseString);
			} catch (WebException wE) {
				// Allow "Bad request" this means we already deleted it
				if (wE.Status == WebExceptionStatus.ProtocolError) {
					if (((HttpWebResponse)wE.Response).StatusCode != HttpStatusCode.BadRequest) {
						throw wE;
					}
				}
			}
			// Make sure we remove it from the history, if no error occured
			config.runtimeFogBugzHistory.Remove(FogBugzInfo.Hash);
			config.FogBugzUploadHistory.Remove(FogBugzInfo.Hash);
			FogBugzInfo.Image = null;
		}
	}
}
