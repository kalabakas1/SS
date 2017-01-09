using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Interfaces;
using MPE.SS.Models;

namespace MPE.SS.Logic.Conductors
{
    internal class RequestConductor : IRequestConductor
    {
        private List<IRequestTest> _serverTests;
        public RequestConductor(
            IEnumerable<IRequestTest> serverTest)
        {
            _serverTests = serverTest.ToList();
        }

        public ReportItem ExecuteTests(Request request, ReportItem context)
        {
            context.Header = request.Label;
            context.Elaboration = request.Uri;
            Parallel.ForEach(_serverTests, requestText =>
            {
                var subContext = context.Create();
                requestText.Run(request, subContext);
                subContext.Update();
            });

            return context;
        }
    }
}
