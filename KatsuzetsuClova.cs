using KatsuzetsuApp.Settings;
using LineDC.CEK;
using LineDC.CEK.Models;
using LineDC.Messaging;
using LineDC.Messaging.Messages;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KatsuzetsuApp
{
    public class KatsuzetsuClova : ClovaBase, ILineMessageableClova
    {
        public ILineMessagingClient LineMessagingClient { get; set; }

        protected override Task OnLaunchRequestAsync(Session session, CancellationToken cancellationToken)
        {
            Response
                .AddText(Messages.WelcomeMessage)
                .KeepListening();

            return Task.CompletedTask;
        }

        protected override async Task OnIntentRequestAsync(Intent intent, Session session, CancellationToken cancellationToken)
        {
            switch (intent.Name)
            {
                case IntentNames.DefaultIntent:
                    {
                        // 滑舌オーケー
                        if (intent.Slots.TryGetValue(SlotNames.NamamugiNamagomeNamatamago, out var slot) ||
                            intent.Slots.TryGetValue(SlotNames.SyusyaSentaku, out slot))
                        {
                            // LINE messaging に投げる
                            await LineMessagingClient.PushMessageAsync(
                                to: session.User.UserId,
                                messages: new List<ISendMessage>
                                {
                                    new TextMessage(string.Format(Messages.LineCongratsMessage, slot.Value)),
                                }
                            );
                            // Clova に喋らせる
                            Response
                                .AddText(Messages.GenerateCongratsMessage(slot.Value))
                                .KeepListening();
                        }
                        // 滑舌ダメです
                        else
                        {
                            await LineMessagingClient.PushMessageAsync(
                                to: session.User.UserId,
                                messages: new List<ISendMessage>
                                {
                                    new TextMessage(Messages.LineWrongMessage),
                                }
                            );

                            Response
                                .AddText(Messages.WrongPronunciationMessage)
                                .KeepListening();
                        }

                        break;
                    }
                default:
                    // noop
                    break;
            }
        }

        protected override Task OnSessionEndedRequestAsync(Session session, CancellationToken cancellationToken)
        {
            Response.AddText(Messages.GoodbayMessage);
            return Task.CompletedTask;
        }
    }
}
