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

			EnsureFieldOrder(list);
		}

		private void EnsureFields(SPList list)
		{
			list.Fields.SuppressTitle();

			// We can't use the title 'Order' as a field with that title already
			// exists on every SharePoint list
			SPFieldLookup order = list.Fields.EnsureLookup("Orders", "Order ID");
			order.LookupField = "ID";
			order.Required = true;
			order.Indexed = true;
			order.RelationshipDeleteBehavior = SPRelationshipDeleteBehavior.Cascade;
			order.Update();

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

		private void EnsureFieldOrder(SPList list)
		{
			string[] fieldOrder =
			{
				"Order ID",
				"Product",
				"Unit Price",
				"Quantity",
				"Discount"
			};

			list.Fields.Reorder(fieldOrder);

			list.Views.EnsureDefaultView(fieldOrder);
		}

		public void TearDown(SPWeb web)
		{
			web.Lists.Delete(LIST_TITLE);
		}
	}
}
