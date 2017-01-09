using System.Net;
using MPE.SS.Enums;
using MPE.SS.Interfaces;
using MPE.SS.Logic.Tests.Base;
using MPE.SS.Models;

namespace MPE.SS.Logic.Tests.Requests
{
    internal class StatusRequestTest : RequestTestAbstract
    {
        private static object _lock = new object();
        public override string Name { get { return "Request status expected {0} got {1}"; } }

        public override ReportItem Run(Request request, ReportItem context)
        {
            lock (_lock)
            {
                context.State = ReportItemState.Failure;

                HttpWebResponse response;
                try
                {
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                    var webRequest = WebRequest.CreateHttp(request.Uri);
                    webRequest.Headers.Add(AcceptEncodingHeader, "gzip");

                    response = (HttpWebResponse)webRequest.GetResponse();
                }
                catch (WebException ex)
                {
                    response = (HttpWebResponse)ex.Response;
                }

                if (response != null)
                {
                    if ((int)response.StatusCode == request.StatusCode)
                    {
                        context.State = ReportItemState.Success;
                    }
                    context.Header = string.Format(Name, request.StatusCode, (int)response.StatusCode);
                }
                else
                {
                    context.Header = string.Format(Name, request.StatusCode, "null");
                }

                return context;
            }
        }
    }
}
