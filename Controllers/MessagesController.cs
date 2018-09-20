using System;
using System.Threading.Tasks;
using System.Web.Http;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using System.Net.Http;
using System.Web.Http.Description;
using System.Diagnostics;
using FormBot.Models;

namespace Microsoft.Bot.Sample.FormBot
{
	[BotAuthentication]
	public class MessagesController : ApiController
	{
		/// <summary>
		/// POST: api/Messages
		/// receive a message from a user and send replies
		/// </summary>
		/// <param name="activity"></param>
		[ResponseType(typeof(void))]
		public virtual async Task<HttpResponseMessage> Post([FromBody] Activity activity)
		{
			if (activity != null)
			{
				// one of these will have an interface and process it
				switch (activity.GetActivityType())
				{
					case ActivityTypes.Message:
						try
						{
							await Conversation.SendAsync(activity, () => new RootDialog());
							break;

						}
						catch (FormCanceledException<DynaForm> e)
						{
							Console.WriteLine(e);
							throw;
						}

					case ActivityTypes.ConversationUpdate:
					case ActivityTypes.ContactRelationUpdate:
					case ActivityTypes.Typing:
					case ActivityTypes.DeleteUserData:
					default:
						Trace.TraceError($"Unknown activity type ignored: {activity.GetActivityType()}");
						break;
				}
			}

			return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
		}
	}

	[Serializable]
	internal class RootDialog : IDialog<object>
	{
		public Task StartAsync(IDialogContext context)
		{
			context.Wait(MessageReceivedAsync);

			return Task.CompletedTask;
		}

		private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
		{
			var message = await result;
			var FormDialog = MakeRootDialog();
			context.Call(FormDialog, FormSubmitted);
		}

		internal static IFormDialog<DynaForm> MakeRootDialog()
		{
			return FormDialog.FromForm(DynaForm.BuildFormAsync, FormOptions.PromptInStart);
		}

		public async Task FormSubmitted(IDialogContext context, IAwaitable<DynaForm> result)
		{
			try
			{
				var form = await result;
				await context.PostAsync("Thanks for your response.");
			}
			catch (FormCanceledException<DynaForm> e)
			{
				string reply;
				if (e.InnerException == null)
				{
					reply = $"Thanks for filling out the form.";

				}
				else
				{
					reply = $"Sorry, I've had a short circuit.  Please try again.";
				}

				context.Done(true);
				await context.PostAsync(reply);
			}
		}
	}
}