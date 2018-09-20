using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.FormFlow.Advanced;
using FormBot.Models;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading.Tasks;

namespace FormBot.Models
{
	[Serializable]
	public class DynaForm
	{
		public static List<Customer> CustomerAccounts { get; set; }
		public Customer Source { get; set; }
		public Customer Destination { get; set; }

		public static IForm<DynaForm> BuildFormAsync()
		{
			//CustomerAccounts = new List<string>()
			//{
			//    "Customer Account 1",
			//    "Customer Account 2",
			//    "Customer Account 3",
			//    "Customer Account 4",
			//};


			CustomerAccounts=new List<Customer>()
			{
				new Customer(){Name = "Customer Account 1"},
				new Customer(){Name = "Customer Account "},
				new Customer(){Name = "Customer Account 3"}
			};

			return new FormBuilder<DynaForm>()
				.Message("Welcome to the simple sandwich order bot!")
				.Field(new FieldReflector<DynaForm>(nameof(Source))
					.SetType(null)
					.SetDefine((state, field) => {
						foreach (var account in CustomerAccounts)
						{
							field
								.AddDescription(account, account.Name)
								.AddTerms(account, account.Name);
						}

						return Task.FromResult(true);
					})
					.SetPrompt(new PromptAttribute("Select the source account \n {||} \n")
					{
						ChoiceStyle = ChoiceStyleOptions.Buttons
					})
					.SetAllowsMultiple(false)
				)
				.Field(new FieldReflector<DynaForm>(nameof(Destination))
					.SetType(null)
					.SetDependencies(nameof(Source))
					.SetDefine((state, field) =>
					{
						foreach (var account in CustomerAccounts)
						{
							field
								.AddDescription(account, account.Name)
								.AddTerms(account, account.Name);
						}
						return Task.FromResult(true);
					})
					.SetPrompt(new PromptAttribute("Select the destination account \n {||} \n") { ChoiceStyle = ChoiceStyleOptions.Buttons })
					.SetAllowsMultiple(false)
				)
				.Confirm("Do you want to continue? {||}")
				.OnCompletion(async (context, order) =>
				{
					await context.PostAsync("Excellent! Your order has been placed. :)");
				})
				.Build();
		}
	}

}