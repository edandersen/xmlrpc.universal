using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Web;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Windows.Web.Http;
using Windows.Data.Xml.Dom;
using XmlRpcPortable.Models;

namespace XmlRpcPortable
{
    public class XmlRpcClient
    {
        private Uri _uri;

        public static string Useragent { get; set; }

        public XmlRpcClient(Uri uri)
        {
            _uri = uri;
        }

        public async Task<XmlRpcResponse> CallRpc(string methodName, List<XmlRpcValue> parameters) {

            var sb = new StringBuilder();

            var writer = XmlWriter.Create(sb);
            writer.WriteStartDocument();

            writer.WriteStartElement("methodCall");

            writer.WriteElementString("methodName", methodName);


            if (parameters != null && parameters.Count() > 0)
            {
                writer.WriteStartElement("params");

                foreach (var parm in parameters)
                {
                    writer.WriteStartElement("param");

                    writer.WriteStartElement("value");

                    parm.BuildXml(writer);

                    writer.WriteEndElement();

                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }

            writer.WriteEndElement();

            writer.WriteEndDocument();
            writer.Flush();
            writer.Dispose();

            try
            {
                var client = new HttpClient();

                if (Useragent != null)
                {
                    client.DefaultRequestHeaders.UserAgent.Add(new Windows.Web.Http.Headers.HttpProductInfoHeaderValue(Useragent));
                }

                var inputPars = sb.ToString();

                var result = await client.PostAsync(_uri, new HttpStringContent(inputPars, Windows.Storage.Streams.UnicodeEncoding.Utf8, "text/xml"));

                var results = await result.Content.ReadAsStringAsync();

                result.EnsureSuccessStatusCode();

                result.Dispose();
                client.Dispose();

                sb.Clear();

                var resultDoc = new Windows.Data.Xml.Dom.XmlDocument();
                var settings = new XmlLoadSettings()
                {
                    ElementContentWhiteSpace = false
                };

                resultDoc.LoadXml(results.Trim(), settings);

                var node = resultDoc.SelectSingleNode("methodResponse");

                if (node != null)
                {
                    var ret = new XmlRpcResponse(node);
                    return ret;
                }
                throw new XmlRpcException(801, "Response failed to return proper XML response");

            }
            catch (Exception ex)
            {
                if (ex is XmlRpcException)
                {
                    throw ex;
                }
                else
                {
                    throw new XmlRpcException(800, ex.Message);
                }
            }

            return null;
        }

    }
}
