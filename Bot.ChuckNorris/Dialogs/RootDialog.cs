using System;
using System.Threading.Tasks;
using Bot.ChuckNorris.BusinessServices;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

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

            var returnMessage = string.Empty;
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
    }
}