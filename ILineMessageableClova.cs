using Microsoft.Extensions.Logging;
using LineDC.CEK;
using LineDC.Messaging;

namespace KatsuzetsuApp
{
    public interface ILineMessageableClova : IClova
    {
        ILineMessagingClient LineMessagingClient { get; set; }
    }
}
