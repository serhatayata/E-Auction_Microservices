using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.PipelineBehaviours
{
    public class PerformanceBehaviour<TRequest,TResponse> : IPipelineBehavior<TRequest,TResponse>
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;
        public PerformanceBehaviour(ILogger<TRequest> logger)
        {
            _timer = new Stopwatch();
            _logger = logger;
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _timer.Start(); //Ölçümleme başlar
            var response = await next(); //Delegeyi execute ettik.
            _timer.Stop();
            //timer üzerinden aldığımız veri ile loglama yaptık.
            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            if (elapsedMilliseconds > 500)
            {
                var requestName = typeof(TRequest).Name;
                _logger.LogWarning("Long Running Request : {Name} ({ElapsedMilliseconds} milliseconds) {@Request}");
            }

            return response;
        }
    }
}
