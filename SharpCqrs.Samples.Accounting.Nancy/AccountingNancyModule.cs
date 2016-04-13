using System;
using System.Threading;
using System.Threading.Tasks;
using Nancy;
using Nancy.ModelBinding;
using SimpleLogging.Core;

namespace SharpCqrs.Samples.Accounting.Nancy
{
    public class AccountingNancyModule : NancyModule
    {
        private readonly ILoggingService _loggingService;

        public AccountingNancyModule(ILoggingService loggingService)
        {
            _loggingService = loggingService;

            Post["/cmd/account/create", true] = PostCreate;
        }

        private async Task<dynamic> PostCreate(dynamic _, CancellationToken ct)
        {
            try
            {
                _loggingService.Trace(Context.Request.Url);

                var binding = this.Bind<CreateBinding>();
                string accountId = Guid.NewGuid().ToString("D");

                var command = new Create(accountId, binding.Owner);
                Negotiate.
                return Response.AsJson(command, HttpStatusCode.Accepted);
            }
            catch (Exception ex)
            {
                _loggingService.Error(ex, Context.Request.Url);

                return Response.AsJson(string.Empty, HttpStatusCode.InternalServerError);
            }
        }
    }

    public class CreateBinding
    {
        public string Owner { get; set; }
    }
}