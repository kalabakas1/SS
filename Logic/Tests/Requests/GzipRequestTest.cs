using System.Linq;
using System.Net;
using MPE.SS.Enums;
using MPE.SS.Logic.Configurations;
using MPE.SS.Logic.Tests.Base;
using MPE.SS.Models;
using NLog;

namespace MPE.SS.Logic.Tests.Requests
{
    internal class GzipRequestTest : RequestTestAbstract
    {
        private static object _lock = new object();
        public override string Name { get { return "Gzip - "; } }

        public override ReportItem Run(Request request, ReportItem context)
        {
            lock (_lock)
            {
                if (!request.Gzip)
                {
                    context.Delete = true;
                    return context;
                }

                context.State = ReportItemState.Failure;
                context.Header = string.Format("{0} {1}", Name, State.Disabled);

                WebResponse response;
                try
                {
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                    var webRequest = WebRequest.CreateHttp(request.Uri);
                    webRequest.Headers.Add(AcceptEncodingHeader, "gzip");
                    webRequest.Timeout = DefaultTimeout;

                    response = webRequest.GetResponse();
                }
                catch (WebException ex)
                {
                    response = ex.Response;
                    AppConfiguration.Logger.Log(LogLevel.Fatal, ex);
                }

                if (response != null && response.Headers.AllKeys.Any(x => x == ContentEncodingHeader))
                {
                    var header = response.Headers.Get(ContentEncodingHeader);
                    if (header.Contains("gzip"))
                    {
                        context.State = ReportItemState.Success;
                        context.Header = string.Format("{0} {1}", Name, State.Enabled);
                    }
                }

                return context;
            }
        }
    }
}
