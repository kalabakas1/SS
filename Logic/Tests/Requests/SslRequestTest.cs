using System.Net;
using MPE.SS.Enums;
using MPE.SS.Logic.Tests.Base;
using MPE.SS.Models;

namespace MPE.SS.Logic.Tests.Requests
{
    internal class SslRequestTest : RequestTestAbstract
    {
        private static object _lock = new object();
        public override string Name { get { return "SSL is enabled"; } }

        public override ReportItem Run(Request request, ReportItem context)
        {
            lock (_lock)
            {
                if (!request.Ssl)
                {
                    context.Delete = true;
                    return context;
                }

                WebResponse response = null;
                var sslUri = GenerateUri(request);
                try
                {
                    var webRequest = WebRequest.CreateHttp(sslUri);
                    response = webRequest.GetResponse();
                }
                catch (WebException e)
                {
                    if (e.Status == WebExceptionStatus.TrustFailure
                        || e.Status == WebExceptionStatus.ProtocolError)
                    {
                        context.Header = "SSL is enabled";
                        context.Elaboration = e.Message;
                        context.State = ReportItemState.Warning;
                    }
                    else
                    {
                        context.Header = "SSL is disabled";
                        context.Elaboration = e.Message;
                        context.State = ReportItemState.Failure;
                    }
                }

                if (response != null)
                {
                    context.Header = "SSL is enabled";
                    context.State = ReportItemState.Success;
                }

                return context;
            }
        }
    }
}
