using LineDC.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace KatsuzetsuApp
{
    public class ClovaEndpoint
    {
        private readonly ILineMessageableClova _clova;

        public ClovaEndpoint(ILineMessageableClova clova, ILineMessagingClient lineMessagingClient)
        {
            _clova = clova;
            _clova.LineMessagingClient = lineMessagingClient;
        }

        [FunctionName("clova")]
        public async Task<IActionResult> Clova(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var response = await _clova.RespondAsync(req.Headers["SignatureCEK"], req.Body);
            return new OkObjectResult(response);
        }
    }
}
