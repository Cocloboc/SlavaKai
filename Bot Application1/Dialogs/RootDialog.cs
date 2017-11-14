using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Text.RegularExpressions;
using System.Linq;

namespace Bot_Application1.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        protected int Sys_Number { get; set; }
        protected int First_Number { get; set; }
        protected int Second_Number { get; set; }


        public async Task StartAsync(IDialogContext context)
        {

            context.Wait(MessageReceivedStart);
        }
        public async Task MessageReceivedStart(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            await context.PostAsync("Что хочешь?");
            //await context.PostAsync("Hello ! WelCome to Gohul Solutions ", "Please Enter You Name ");
            //context.Wait();



            context.Wait(MessageReceivedOperationChoice);
        }

        public async Task MessageReceivedOperationChoice(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;

            if (message.Text.ToLower().Equals("конвертер", StringComparison.InvariantCultureIgnoreCase))
            {
                await context.PostAsync("Что с основанием системы счисления?(2, 8, 10 или 16)");
                context.Wait(conv_Sys);
            }
            else if (message.Text.ToLower().Equals("калькулятор", StringComparison.InvariantCultureIgnoreCase))
            {
                await context.PostAsync("Что с основанием системы счисления первого числа?(2, 8, 10, 16)");
                context.Wait(calc_Sys1);
            }
            else
            {
                await context.PostAsync("Так, понятно. Сложновато для тебя. Ничего, привыкай. Давай сначала.");
                context.Wait(MessageReceivedStart);
            }
        }

        public async Task conv_Sys(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var numbers = await argument;
            this.Sys_Number = int.Parse(numbers.Text);
            if (Sys_Number != 2 & Sys_Number != 8 & Sys_Number != 10 & Sys_Number != 16)
            {
                await context.PostAsync("4 варианта и ты ошибся? Страдай.");
                await MessageReceivedStart(context, argument);
                return;

            }
            await context.PostAsync("А число какое?");
            context.Wait(First_Num);
        }

        bool FormatValid(string format, string Check)
        {
            string allowableLetters = Check;

            foreach (char c in format)
            {
                // This is using String.Contains for .NET 2 compat.,
                //   hence the requirement for ToString()
                if (!allowableLetters.Contains(c.ToString()))
                    return false;
            }

            return true;
        }

        public async Task First_Num(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var numbers = await argument;
            switch (Sys_Number)
            {
                case 2:
                    if (!numbers.Text.All(c => "01".Contains(c)))
                    {
                        await context.PostAsync("Нет, совсем нет. Давай заново.");
                        await MessageReceivedStart(context, argument);
                        return;
                    }
                    break;
                case 8:
                    if (!numbers.Text.All(c => "01234567".Contains(c)))
                    {
                        await context.PostAsync("Нет, совсем нет. Давай заново.");
                        await MessageReceivedStart(context, argument);
                        return;
                    }
                    break;
                case 10:
                    if (!numbers.Text.All(c => "0123456789".Contains(c)))
                    {
                        await context.PostAsync("Нет, совсем нет. Давай заново.");
                        await MessageReceivedStart(context, argument);
                        return;
                    }
                    break;
                case 16:
                    if (!numbers.Text.All(c => "0123456789ABCDEF".Contains(c)))
                    {
                        await context.PostAsync("Нет, совсем нет. Давай заново.");
                        await MessageReceivedStart(context, argument);
                        return;
                    }
                    break;
            }
                First_Number = Convert.ToInt32(numbers.Text, Sys_Number);
            await context.PostAsync("В какую систему переводим?(2, 8, 10 или 16)");
            context.Wait(conv);
        }
        public async Task conv(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var numbers = await argument;
            this.Sys_Number = int.Parse(numbers.Text);
            if (Sys_Number != 2 & Sys_Number != 8 & Sys_Number != 10 & Sys_Number != 16)
            {
                await context.PostAsync("4 варианта и ты ошибся? Страдай.");
                await MessageReceivedStart(context, argument);
                return;
            }
            String result = Convert.ToString(First_Number, Sys_Number);
            await context.PostAsync(result);
            await MessageReceivedStart(context, argument);
        }

        public async Task calc_Sys1(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var numbers = await argument;
            this.Sys_Number = int.Parse(numbers.Text);
            if (Sys_Number != 2 & Sys_Number != 8 & Sys_Number != 10 & Sys_Number != 16)
            {
                await context.PostAsync("4 варианта и ты ошибся? Страдай.");
                await MessageReceivedStart(context, argument);
                return;

            }
            await context.PostAsync("А первое число какое?");
            context.Wait(First_Num_Calc);
        }
        public async Task First_Num_Calc(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var numbers = await argument;
            switch (Sys_Number)
            {
                case 2:
                    if (!numbers.Text.All(c => "01".Contains(c)))
                    {
                        await context.PostAsync("Нет, совсем нет. Давай заново.");
                        await MessageReceivedStart(context, argument);
                        return;
                    }
                    break;
                case 8:
                    if (!numbers.Text.All(c => "01234567".Contains(c)))
                    {
                        await context.PostAsync("Нет, совсем нет. Давай заново.");
                        await MessageReceivedStart(context, argument);
                        return;
                    }
                    break;
                case 10:
                    if (!numbers.Text.All(c => "0123456789".Contains(c)))
                    {
                        await context.PostAsync("Нет, совсем нет. Давай заново.");
                        await MessageReceivedStart(context, argument);
                        return;
                    }
                    break;
                case 16:
                    if (!numbers.Text.All(c => "0123456789ABCDEF".Contains(c)))
                    {
                        await context.PostAsync("Нет, совсем нет. Давай заново.");
                        await MessageReceivedStart(context, argument);
                        return;
                    }
                    break;
            }
            First_Number = Convert.ToInt32(numbers.Text, Sys_Number);
            await context.PostAsync("Что с системой второго числа?(2, 8, 10 или 16)");
            context.Wait(calc_Sys2);
        }

        public async Task calc_Sys2(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var numbers = await argument;
            this.Sys_Number = int.Parse(numbers.Text);
            if (Sys_Number != 2 & Sys_Number != 8 & Sys_Number != 10 & Sys_Number != 16)
            {
                await context.PostAsync("4 варианта и ты ошибся? Страдай.");
                await MessageReceivedStart(context, argument);
                return;

            }
            await context.PostAsync("А второе число какое?");
            context.Wait(Second_Num_Calc);
        }
        public async Task Second_Num_Calc(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var numbers = await argument;
            switch (Sys_Number)
            {
                case 2:
                    if (!numbers.Text.All(c => "01".Contains(c)))
                    {
                        await context.PostAsync("Нет, совсем нет. Давай заново.");
                        await MessageReceivedStart(context, argument);
                        return;
                    }
                    break;
                case 8:
                    if (!numbers.Text.All(c => "01234567".Contains(c)))
                    {
                        await context.PostAsync("Нет, совсем нет. Давай заново.");
                        await MessageReceivedStart(context, argument);
                        return;
                    }
                    break;
                case 10:
                    if (!numbers.Text.All(c => "0123456789".Contains(c)))
                    {
                        await context.PostAsync("Нет, совсем нет. Давай заново.");
                        await MessageReceivedStart(context, argument);
                        return;
                    }
                    break;
                case 16:
                    if (!numbers.Text.All(c => "0123456789ABCDEF".Contains(c)))
                    {
                        await context.PostAsync("Нет, совсем нет. Давай заново.");
                        await MessageReceivedStart(context, argument);
                        return;
                    }
                    break;
            }
            Second_Number = Convert.ToInt32(numbers.Text, Sys_Number);
            await context.PostAsync("И что с ними делать?(+, -, / или *)");
            context.Wait(Function);
        }
        public async Task Function(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var numbers = await argument;
            if (numbers.Text != "+" & numbers.Text != "-" & numbers.Text != "/" & numbers.Text != "*")
            {
                await context.PostAsync("4 варианта и ты ошибся? Страдай.");
                await MessageReceivedStart(context, argument);
                return;

            }
            switch (numbers.Text)
            {
                case "+": Second_Number = First_Number + Second_Number;
                    break;
                case "-":
                    Second_Number = First_Number - Second_Number;
                    break;
                case "*":
                    Second_Number = First_Number * Second_Number;
                    break;
                case "/":
                    Second_Number = First_Number / Second_Number;
                    break;
            }
            await context.PostAsync("В какой системе хочешь ответ?(2, 8, 10, 16)");
            context.Wait(Result);
        }
        public async Task Result(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var numbers = await argument;
            this.Sys_Number = int.Parse(numbers.Text);
            if (Sys_Number != 2 & Sys_Number != 8 & Sys_Number != 10 & Sys_Number != 16)
            {
                await context.PostAsync("4 варианта и ты ошибся? Страдай.");
                await MessageReceivedStart(context, argument);
                return;

            }
            if (Sys_Number == 10) await context.PostAsync(Second_Number.ToString());           
            else
            {
                if (Second_Number < 0)
                    await context.PostAsync("Ты не хочешь этого видеть");
                else
                await context.PostAsync(Convert.ToString(Second_Number, Sys_Number));
            }
            await MessageReceivedStart(context, argument);
        }

    }
}
