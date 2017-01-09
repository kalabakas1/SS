using System.Net;
using MPE.SS.Interfaces;
using MPE.SS.Logic.Configurations;
using MPE.SS.Models;
using NLog;

namespace MPE.SS.Logic.Tests.Base
{
    internal abstract class RequestTestAbstract : IRequestTest
    {
        protected const string ContentEncodingHeader = "Content-Encoding";
        protected const string AcceptEncodingHeader = "Accept-Encoding";
        protected const int DefaultTimeout = 15000;

        public abstract ReportItem Run(Request request, ReportItem context);

        protected string GenerateUri(Request request)
        {
            var uri = request.Uri.Replace("https://", string.Empty).Replace("http://", string.Empty);
            return (request.Ssl ? "https" : "http") + "://" + uri;
        }

        public abstract string Name { get; }
    }
}
