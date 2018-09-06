using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.CognitiveServices.QnAMaker;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace SimpleQnABot.Dialogs
{
    [Serializable]
    // Below method uses the V2 APIs : https://aka.ms/qnamaker-v2-apis. 
    // To use V4 stack, you also need to add the Endpoint hostname to the parameters below : https://aka.ms/qnamaker-v4-apis
    //[QnAMaker("set yout subscription key here", "set your kbid here")]
    public class SimpleQnADialog : QnAMakerDialog
    { 
        public SimpleQnADialog() : base(new QnAMakerService(
            new QnAMakerAttribute(
                ConfigurationManager.AppSettings["QnaSubscriptionKey"], 
                ConfigurationManager.AppSettings["QnaKnowledgebaseId"], 
                "Sorry, I couldn't find an answer for that", 
                0.05,
                endpointHostName: "https://etradebotqna.azurewebsites.net/qnamaker")))
        {
        }

        protected override async Task RespondFromQnAMakerResultAsync(IDialogContext context, IMessageActivity message, QnAMakerResults result)
        {
            // Add code to format QnAMakerResults 'result'

            // Answer is a string
            string title = $"#### {result.Answers.First().Questions.First()}";
            string answer = result.Answers.First().Answer;
            string score = result.Answers.First().Score.ToString();

            string imageMD = "![ETrade](https://cdn.etrade.net/1/17092613100.0/aempros/content/dam/etrade/retail/en_US/images/global/logos/etrade-logo-rgb-144x22.svg)";

            IMessageActivity replyMessage = context.MakeMessage();
            replyMessage.Text = $"{imageMD}\n\n{title}\n\nAnswer score:{score}\n\n{answer}";
            replyMessage.TextFormat = "markdown";
            replyMessage.Locale = "en-Us";
            
            //replyMessage.Attachments.Add(new Attachment()
            //{
            //    ContentUrl = "https://cdn.etrade.net/1/17092613100.0/aempros/content/dam/etrade/retail/en_US/images/global/logos/etrade-logo-rgb-144x22.svg",
            //    ContentType = "image/svg+xml",
            //    Name = "etrade_logo.svg"
            //});

            await context.PostAsync(replyMessage, CancellationToken.None);

            

            
        }
    }
}