using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text; 
using System.Threading; 

namespace SqlViewerNetwork.NetworkServer
{
    public class HttpNetworkServer : INetworkServer
    {
        #region Private properties
        /// <summary>
        /// Instance of HttpListener
        /// </summary>
        private HttpListener Listener { get; set; }

        /// <summary>
        /// Web site's file system delimiter characters 
        /// </summary>
        private char[] WebSiteFSDelimiterChars { get; } = { '/', '\\' }; 

        /// <summary>
        /// Path of bin directory 
        /// </summary>
        private string BinPath { get; } = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location); 

        /// <summary>
        /// Allowed web site's paths 
        /// </summary>
        private Dictionary<string, string> WebPaths = new Dictionary<string, string>();
        #endregion  // Private properties

        #region Constructors
        /// <summary>
        /// Default constructor 
        /// </summary>
        public HttpNetworkServer()
        {
            AddWebPaths(); 
        }
        #endregion  // Constructors

        #region Public methods
        public bool IsRunning()
        {
            return Listener != null; 
        }

        /// <summary>
        /// Create web server as HttpListener
        /// </summary>
        public void StartServer()
        {
            try
            {
                Listener = new HttpListener();
                AddPrefixes(); 
                Listener.Start();
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }

        /// <summary>
        /// Stops the server  
        /// </summary>
        public void StopServer()
        {
            try
            {
                if (Listener != null) Listener.Stop(); 
                Listener = null; 
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }
        #endregion  // Public methods

        #region Request processing 
        /// <summary>
        /// Initializes thread for processing HTTP requests 
        /// </summary>
        public void Listen()
        {
            if (Listener == null) throw new System.Exception("Unable to listen because Listener is null"); 
            try
            {
                ThreadPool.QueueUserWorkItem((_) => ProcessRequest(Listener.GetContext()));
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }

        /// <summary>
        /// Processes request and sends response back 
        /// </summary>
        /// <param name="ctx">Instance of HttpListenerContext</param>
        private void ProcessRequest(HttpListenerContext ctx)
        {
            if (ctx == null) throw new System.Exception("HttpListenerContext is null"); 
            try
            {
                string responseText = GetResponseText(ctx.Request.Url.ToString());
                byte[] buf = Encoding.UTF8.GetBytes(responseText);

                ctx.Response.ContentEncoding = Encoding.UTF8;
                ctx.Response.ContentType = "text/html";
                ctx.Response.ContentLength64 = buf.Length;

                ctx.Response.OutputStream.Write(buf, 0, buf.Length);
                ctx.Response.Close();
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        }

        /// <summary>
        /// Gets response text depending on a given URL 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string GetResponseText(string url)
        {
            if (IsPathValid(url, WebPaths["firstExample"]))
            {
                return ReadAllTextFromBinDir(@"\pages\firstExample.html");
            }
            else if (IsPathValid(url, WebPaths["test"]))
            {
                return ReadAllTextFromBinDir(@"\pages\test.html");
            }
            else if (IsPathValid(url, WebPaths["dbg"]))
            {
                return "<html><head><title>Debug</title></head><body>Hello, this is a custom Server.<br>Debug</body></html>";
            }
            return "Page is not found";
        }
        #endregion  // Request processing 

        #region Web site's folder structure
        /// <summary>
        /// Adds all the possible paths into WebPaths dictionary 
        /// </summary>
        private void AddWebPaths()
        {
            WebPaths.Add("firstExample", "/firstExample/");
            WebPaths.Add("test", "/test/");
            WebPaths.Add("dbg", "/dbg/");
        }

        /// <summary>
        /// Adds all the possible paths from WebPaths dictionary into HttpListener.Prefixes 
        /// </summary>
        /// <param name="Listener"></param>
        private void AddPrefixes()
        {
            foreach (string key in WebPaths.Keys)
            {
                Listener.Prefixes.Add("http://localhost:4444/" + WebPaths[key].TrimStart(WebSiteFSDelimiterChars).TrimEnd(WebSiteFSDelimiterChars) + "/");
            }
        }

        /// <summary>
        /// Checks if web site path is valid 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool IsPathValid(string url, string path)
        {
            return System.IO.File.Exists(path) && url.Contains(path.TrimStart(WebSiteFSDelimiterChars).TrimEnd(WebSiteFSDelimiterChars));
        }
        #endregion  // Web site's folder structure

        #region Getting files 
        /// <summary>
        /// Reads all text from specified file located inside bin directory 
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        private string ReadAllTextFromBinDir(string filepath)
        {
            return System.IO.File.Exists(filepath) ? System.IO.File.ReadAllText(BinPath.TrimEnd(WebSiteFSDelimiterChars) + @"\" + filepath.TrimStart(WebSiteFSDelimiterChars)) : string.Empty; 
        }
        #endregion  // Getting files 
    }
}
