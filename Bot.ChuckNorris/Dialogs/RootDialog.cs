using System;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Bot.ChuckNorris.BusinessServices;

namespace Bot.ChuckNorris.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private readonly IChuckNorrisService _chuckNorrisService;

        public RootDialog(IChuckNorrisService chuckNorrisService)
        {
            _chuckNorrisService = chuckNorrisService;
        }

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if (!message.Text.ToLower().StartsWith("chuck"))
                return;

            //Simulate Bot Typing
            await AddTypingActivityAsync((Activity)message);

            string returnMessage;
            if (message.Text.ToLower().Equals("chuck ping"))
            {
                returnMessage = "pong";
            }
            else
            {
                returnMessage = _chuckNorrisService.FindBestFact(message.Text);
            }

            await context.PostAsync(returnMessage);
        }

        private async Task AddTypingActivityAsync(Activity activity)
        {
            var connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            var isTypingReply = activity.CreateReply();
            isTypingReply.Type = ActivityTypes.Typing;
            isTypingReply.Text = null;

            await connector.Conversations.ReplyToActivityAsync(isTypingReply, CancellationToken.None);

            var rnd = new Random();
            var waitMilliSeconds = rnd.Next(600, 1600);

            await System.Threading.Tasks.Task.Delay(waitMilliSeconds, CancellationToken.None);
        }
    }
}