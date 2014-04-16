using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;
using Northwind.Common.Lists;
using Northwind.Common.Lists.Base;

namespace Northwind.Common.Features.Northwind.Common
{
	[Guid("22f20af9-fcc4-4fd0-b968-6808b10b8981")]
	public class NorthwindCommonEventReceiver : SPFeatureReceiver
	{
		private readonly ListManager listManager = new ListManager(new List<IEnsurableList>
		{
			new CustomerDemographics(),
			new Customers(),
			new Regions(),
			new Territories(),
			new Employees(),
			new Shippers(),
			new Orders(),
			new Suppliers(),
			new Categories(),
			new Products(),
			new OrderDetails()
		});

		public override void FeatureActivated(SPFeatureReceiverProperties properties)
		{
			SPSite site = properties.Feature.Parent as SPSite;

			Ensure(site.RootWeb);
		}

		public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, IDictionary<string, string> parameters)
		{
			SPSite site = properties.Feature.Parent as SPSite;

			Ensure(site.RootWeb);
		}

		public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
		{
			SPSite site = properties.Feature.Parent as SPSite;

			TearDown(site.RootWeb);
		}

		private void Ensure(SPWeb web)
		{
			listManager.EnsureLists(web);
		}

		private void TearDown(SPWeb web)
		{
			listManager.TearDownLists(web);
		}
	}
}