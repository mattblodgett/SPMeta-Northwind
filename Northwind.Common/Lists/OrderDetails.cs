using CamlexNET;
using Microsoft.SharePoint;
using Northwind.Common.Extensions;
using Northwind.Common.Lists.Base;

namespace Northwind.Common.Lists
{
	public class OrderDetails : IEnsurableList
	{
		private const string LIST_TITLE = "Order Details";

		public void Ensure(SPWeb web)
		{
			SPList list = web.Lists.Ensure(SPListTemplateType.GenericList, LIST_TITLE);

			EnsureFields(list);

			EnsureViews(list);
		}

		private void EnsureFields(SPList list)
		{
			list.Fields.SuppressTitle();

			// We can't use the title 'Order' as a field with that title already
			// exists on every SharePoint list
			SPFieldLookup orderId = list.Fields.EnsureLookup("Orders", "Order ID");
			orderId.LookupField = "ID";
			orderId.Required = true;
			orderId.Indexed = true;
			orderId.RelationshipDeleteBehavior = SPRelationshipDeleteBehavior.Cascade;
			orderId.Update();

			SPFieldLookup product = list.Fields.EnsureLookup("Products", "Product");
			product.Required = true;
			product.Update();

			SPFieldCurrency unitPrice = list.Fields.Ensure<SPFieldCurrency>("Unit Price");
			unitPrice.Required = true;
			unitPrice.Update();

			SPFieldNumber quantity = list.Fields.Ensure<SPFieldNumber>("Quantity");
			quantity.Required = true;
			quantity.MinimumValue = 0;
			quantity.Update();

			SPFieldNumber discount = list.Fields.Ensure<SPFieldNumber>("Discount");
			discount.Required = true;
			discount.DefaultValue = "0";
			discount.MinimumValue = 0;
			discount.ShowAsPercentage = true;
			discount.Update();
		}

		private void EnsureViews(SPList list)
		{
			SPView defaultView = list.Views.EnsureDefaultView(new[]
			{
				"Order ID",
				"Product",
				"Quantity",
				"Unit Price",
				"Discount"
			});

			string orderIdInternal = list.Fields["Order ID"].InternalName;
			defaultView.Query = Camlex.Query().OrderBy(x => x[orderIdInternal]).ToString();
			defaultView.Update();
		}

		public void TearDown(SPWeb web)
		{
			web.Lists.Delete(LIST_TITLE);
		}
	}
}
