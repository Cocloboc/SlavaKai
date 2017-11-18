using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using AdaptiveCards;

namespace Calcul.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {

        protected int Sys_Number { get; set; }
        protected int First_Number { get; set; }
        protected double First_Number_in { get; set; }
        protected int Second_Number { get; set; }
        protected double Second_Number_in { get; set; }
        protected double tutu { get; set; }


        public async Task StartAsync(IDialogContext context)
        {

            context.Wait(MessageReceivedStart);
        }
        public async Task MessageReceivedStart(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var reply = context.MakeMessage();
            reply.Attachments = new List<Attachment>();
            var heroCard = new HeroCard
            {
                Title = $"Что хочешь?",
                Buttons = new List<CardAction>()
                    {
                        new CardAction()
                        {
                            Value = $"калькулятор",
                            Type = "postBack",
                            Title = "Калькулятор"
                        },
                        new CardAction()
                        {
                            Value = $"конвертер",
                            Type = "postBack",
                            Title = "Конвертер"
                        }
                     }
            };
            reply.Attachments.Add(heroCard.ToAttachment());
            await context.PostAsync(reply);
            context.Wait(MessageReceivedOperationChoice);
        }

        public async Task MessageReceivedOperationChoice(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;

            if (message.Text.ToLower().Equals("конвертер", StringComparison.InvariantCultureIgnoreCase))
            {
                var reply = context.MakeMessage();
                reply.Attachments = new List<Attachment>();
                var heroCard = new HeroCard
                {
                    Title = $"Что с основанием системы счисления?",
                    Buttons = new List<CardAction>()
                    {
                        new CardAction()
                        {
                            Value = $"2",
                            Type = "postBack",
                            Title = "Двоичная"
                        },
                        new CardAction()
                        {
                            Value = $"8",
                            Type = "postBack",
                            Title = "Восьмеричная"
                        },
                        new CardAction()
                        {
                            Value = $"10",
                            Type = "postBack",
                            Title = "Десятичная"
                        },
                        new CardAction()
                        {
                            Value = $"16",
                            Type = "postBack",
                            Title = "Шестнадцатиричная"
                        }
                     }
                };
                reply.Attachments.Add(heroCard.ToAttachment());
                await context.PostAsync(reply);
                context.Wait(conv_Sys);
            }
            else if (message.Text.ToLower().Equals("калькулятор", StringComparison.InvariantCultureIgnoreCase))
            {
                var reply = context.MakeMessage();
                reply.Attachments = new List<Attachment>();
                var heroCard = new HeroCard
                {
                    Title = $"Что с основанием системы счисления?",
                    Buttons = new List<CardAction>()
                    {
                        new CardAction()
                        {
                            Value = $"2",
                            Type = "postBack",
                            Title = "Двоичная"
                        },
                        new CardAction()
                        {
                            Value = $"8",
                            Type = "postBack",
                            Title = "Восьмеричная"
                        },
                        new CardAction()
                        {
                            Value = $"10",
                            Type = "postBack",
                            Title = "Десятичная"
                        },
                        new CardAction()
                        {
                            Value = $"16",
                            Type = "postBack",
                            Title = "Шестнадцатиричная"
                        }
                     }
                };
                reply.Attachments.Add(heroCard.ToAttachment());
                await context.PostAsync(reply);
                context.Wait(calc_Sys1);
            }
            else
            {
                await context.PostAsync("Так, понятно. Сложновато для тебя. Ничего, привыкай. Давай сначала.");
                await MessageReceivedStart(context, argument);
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
                    else
                    {
                        First_Number = Convert.ToInt32(numbers.Text, Sys_Number);
                        First_Number_in = Convert.ToDouble(First_Number);
                    }
                    break;
                case 8:
                    if (!numbers.Text.All(c => "01234567".Contains(c)))
                    {
                        await context.PostAsync("Нет, совсем нет. Давай заново.");
                        await MessageReceivedStart(context, argument);
                        return;
                    }
                    else
                    {
                        First_Number = Convert.ToInt32(numbers.Text, Sys_Number);
                        First_Number_in = Convert.ToDouble(First_Number);
                    }
                    break;
                case 10:
                    if (!numbers.Text.All(c => "0123456789,".Contains(c)))
                    {
                        await context.PostAsync("Нет, совсем нет. Давай заново.");
                        await MessageReceivedStart(context, argument);
                        return;
                    }
                    else
                    {
                        First_Number_in = Convert.ToDouble(numbers.Text);
                    }
                    break;
                case 16:
                    if (!numbers.Text.All(c => "0123456789ABCDEFabcdef".Contains(c)))
                    {
                        await context.PostAsync("Нет, совсем нет. Давай заново.");
                        await MessageReceivedStart(context, argument);
                        return;
                    }
                    else
                    {
                        First_Number = Convert.ToInt32(numbers.Text, Sys_Number);
                        First_Number_in = Convert.ToDouble(First_Number);
                    }
                    break;
            }
            var reply = context.MakeMessage();
            reply.Attachments = new List<Attachment>();
            var heroCard = new HeroCard
            {
                Title = $"В какую систему переводим?",
                Buttons = new List<CardAction>()
                    {
                        new CardAction()
                        {
                            Value = $"2",
                            Type = "postBack",
                            Title = "Двоичная"
                        },
                        new CardAction()
                        {
                            Value = $"8",
                            Type = "postBack",
                            Title = "Восьмеричная"
                        },
                        new CardAction()
                        {
                            Value = $"10",
                            Type = "postBack",
                            Title = "Десятичная"
                        },
                        new CardAction()
                        {
                            Value = $"16",
                            Type = "postBack",
                            Title = "Шестнадцатиричная"
                        }
                     }
            };
            reply.Attachments.Add(heroCard.ToAttachment());
            await context.PostAsync(reply);
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
            if (Sys_Number == 10)
            {
                await context.PostAsync(First_Number_in.ToString());
                await MessageReceivedStart(context, argument);
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
                    else
                    {
                        First_Number = Convert.ToInt32(numbers.Text, Sys_Number);
                        First_Number_in = Convert.ToDouble(First_Number);
                    }
                    break;
                case 8:
                    if (!numbers.Text.All(c => "01234567".Contains(c)))
                    {
                        await context.PostAsync("Нет, совсем нет. Давай заново.");
                        await MessageReceivedStart(context, argument);
                        return;
                    }
                    else
                    {
                        First_Number = Convert.ToInt32(numbers.Text, Sys_Number);
                        First_Number_in = Convert.ToDouble(First_Number);
                    }
                    break;
                case 10:
                    if (!numbers.Text.All(c => "0123456789,".Contains(c)))
                    {
                        await context.PostAsync("Нет, совсем нет. Давай заново.");
                        await MessageReceivedStart(context, argument);
                        return;
                    }
                    else
                    {
                        First_Number_in = Convert.ToDouble(numbers.Text);
                    }
                    break;
                case 16:
                    if (!numbers.Text.All(c => "0123456789ABCDEFabcdef".Contains(c)))
                    {
                        await context.PostAsync("Нет, совсем нет. Давай заново.");
                        await MessageReceivedStart(context, argument);
                        return;
                    }
                    else
                    {
                        First_Number = Convert.ToInt32(numbers.Text, Sys_Number);
                        First_Number_in = Convert.ToDouble(First_Number);
                    }
                    break;
            }
            var reply = context.MakeMessage();
            reply.Attachments = new List<Attachment>();
            var heroCard = new HeroCard
            {
                Title = $"Что с основанием второго числа?",
                Buttons = new List<CardAction>()
                    {
                        new CardAction()
                        {
                            Value = $"2",
                            Type = "postBack",
                            Title = "Двоичная"
                        },
                        new CardAction()
                        {
                            Value = $"8",
                            Type = "postBack",
                            Title = "Восьмеричная"
                        },
                        new CardAction()
                        {
                            Value = $"10",
                            Type = "postBack",
                            Title = "Десятичная"
                        },
                        new CardAction()
                        {
                            Value = $"16",
                            Type = "postBack",
                            Title = "Шестнадцатиричная"
                        }
                     }
            };
            reply.Attachments.Add(heroCard.ToAttachment());
            await context.PostAsync(reply);
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
                    else
                    {
                        Second_Number = Convert.ToInt32(numbers.Text, Sys_Number);
                        Second_Number_in = Convert.ToDouble(Second_Number);
                    }
                    break;
                case 8:
                    if (!numbers.Text.All(c => "01234567".Contains(c)))
                    {
                        await context.PostAsync("Нет, совсем нет. Давай заново.");
                        await MessageReceivedStart(context, argument);
                        return;
                    }
                    else
                    {
                        Second_Number = Convert.ToInt32(numbers.Text, Sys_Number);
                        Second_Number_in = Convert.ToDouble(Second_Number);
                    }
                    break;
                case 10:
                    if (!numbers.Text.All(c => "0123456789".Contains(c)))
                    {
                        await context.PostAsync("Нет, совсем нет. Давай заново.");
                        await MessageReceivedStart(context, argument);
                        return;
                    }
                    else
                    {
                        Second_Number_in = Convert.ToDouble(numbers.Text);
                    }
                    break;
                case 16:
                    if (!numbers.Text.All(c => "0123456789ABCDEFabcdef".Contains(c)))
                    {
                        await context.PostAsync("Нет, совсем нет. Давай заново.");
                        await MessageReceivedStart(context, argument);
                        return;
                    }
                    else
                    {
                        Second_Number = Convert.ToInt32(numbers.Text, Sys_Number);
                        Second_Number_in = Convert.ToDouble(Second_Number);
                    }
                    break;
            }
            var reply = context.MakeMessage();
            reply.Attachments = new List<Attachment>();
            var heroCard = new HeroCard
            {
                Title = $"Что с ними делать?",
                Buttons = new List<CardAction>()
                    {
                        new CardAction()
                        {
                            Value = $"+",
                            Type = "postBack",
                            Title = "Сложи"
                        },
                        new CardAction()
                        {
                            Value = $"-",
                            Type = "postBack",
                            Title = "Вычти"
                        },
                        new CardAction()
                        {
                            Value = $"*",
                            Type = "postBack",
                            Title = "Умножь"
                        },
                        new CardAction()
                        {
                            Value = $"/",
                            Type = "postBack",
                            Title = "Дели"
                        }
                     }
            };
            reply.Attachments.Add(heroCard.ToAttachment());
            await context.PostAsync(reply);
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
                case "+":
                    tutu = First_Number_in + Second_Number_in;
                    break;
                case "-":
                    tutu = First_Number_in - Second_Number_in;
                    break;
                case "*":
                    tutu = First_Number_in * Second_Number_in;
                    break;
                case "/":
                    tutu = First_Number_in / Second_Number_in;
                    break;
            }
            var reply = context.MakeMessage();
            reply.Attachments = new List<Attachment>();
            var heroCard = new HeroCard
            {
                Title = $"В какой системе хочешь ответ?",
                Buttons = new List<CardAction>()
                    {
                        new CardAction()
                        {
                            Value = $"2",
                            Type = "postBack",
                            Title = "В двоичной"
                        },
                        new CardAction()
                        {
                            Value = $"8",
                            Type = "postBack",
                            Title = "В восьмеричной"
                        },
                        new CardAction()
                        {
                            Value = $"10",
                            Type = "postBack",
                            Title = "В десятичной"
                        },
                        new CardAction()
                        {
                            Value = $"16",
                            Type = "postBack",
                            Title = "В шестнадцатиричной"
                        }
                     }
            };
            reply.Attachments.Add(heroCard.ToAttachment());
            await context.PostAsync(reply);
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
            if (Sys_Number == 10) await context.PostAsync(tutu.ToString());
            else
            {
                if (Second_Number_in < 0)
                    await context.PostAsync("Ты не хочешь этого видеть");
                else
                    await context.PostAsync(Convert.ToString(Convert.ToInt32(tutu), Sys_Number));
            }
            await MessageReceivedStart(context, argument);
        }

    }
}