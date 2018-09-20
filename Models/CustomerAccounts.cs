using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FormBot.Models
{
	//public class CustomerAccounts
	//{
	//    public class MerchandiseDB
	//    {
	//        public static List<string> GetCustomerAccounts()
	//        {
	//            return new List<string>()
	//            {
	//                "CustomerAccount1",
	//                "CustomerAccount2",
	//                "CustomerAccount3",
	//                "CustomerAccount4",
	//            };
	//        }
	//    }
	//}
	[Serializable]
	public class Customer
	{
		public string Name { get; set; }
		public string AccountNumber  { get; set; }
	}
}