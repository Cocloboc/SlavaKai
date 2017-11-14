using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Bot_Application1.Dialogs
{
    public class Conv : IDialog<object>
    {
        protected int number1 { get; set; }

        public async Task StartAsync(IDialogContext context)
        {

            context.Wait(MessageReceivedStart);
        }
        public async Task MessageReceivedStart(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            await context.PostAsync("Hello ! Sample Calculator | Do you want to add or square or suared ?  ");
            //await context.PostAsync("Hello ! WelCome to Gohul Solutions ", "Please Enter You Name ");
            //context.Wait();



            //context.Wait(MessageReceivedOperationChoice);
        }
    }
}