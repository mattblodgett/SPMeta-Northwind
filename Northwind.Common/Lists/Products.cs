using Microsoft.SharePoint;
using Northwind.Common.Extensions;
using Northwind.Common.Lists.Base;

namespace Northwind.Common.Lists
{
	public class Products : IEnsurableList
	{
		private const string LIST_TITLE = "Products";

		public void Ensure(SPWeb web)
		{
			SPList list = web.Lists.Ensure(SPListTemplateType.GenericList, LIST_TITLE);

			EnsureFields(list);

			EnsureFieldOrder(list);
		}

		private void EnsureFields(SPList list)
		{
			SPFieldText productName = list.Fields[SPBuiltInFieldId.Title] as SPFieldText;
			productName.Title = "Product Name";
			productName.MaxLength = 40;
			productName.Required = true;
			productName.Update();

			SPList suppliers = list.Lists["Suppliers"];
			SPFieldLookup supplier = list.Fields.EnsureLookup(suppliers.Title, "Supplier");
			supplier.LookupField = suppliers.Fields["Company Name"].InternalName;
			supplier.Update();

			SPFieldLookup category = list.Fields.EnsureLookup("Categories", "Category");

			SPFieldText quantityPerUnit = list.Fields.Ensure<SPFieldText>("Quantity Per Unit");
			quantityPerUnit.MaxLength = 20;
			quantityPerUnit.Update();

			SPFieldCurrency unitPrice = list.Fields.Ensure<SPFieldCurrency>("Unit Price");
			unitPrice.MinimumValue = 0;
			unitPrice.Update();

			SPFieldNumber unitsInStock = list.Fields.Ensure<SPFieldNumber>("Units in Stock");
			unitsInStock.MinimumValue = 0;
			unitsInStock.DisplayFormat = SPNumberFormatTypes.NoDecimal;
			unitsInStock.Update();

			SPFieldNumber unitsOnOrder = list.Fields.Ensure<SPFieldNumber>("Units on Order");
			unitsOnOrder.MinimumValue = 0;
			unitsOnOrder.DisplayFormat = SPNumberFormatTypes.NoDecimal;
			unitsOnOrder.Update();

			SPFieldNumber reorderLevel = list.Fields.Ensure<SPFieldNumber>("Reorder Level");
			reorderLevel.MinimumValue = 0;
			reorderLevel.DisplayFormat = SPNumberFormatTypes.NoDecimal;
			reorderLevel.Update();

			SPFieldBoolean discontinued = list.Fields.Ensure<SPFieldBoolean>("Discontinued");
			discontinued.DefaultValue = "0";
			discontinued.Required = true;
			discontinued.Update();
		}

		private void EnsureFieldOrder(SPList list)
		{
			string[] fieldOrder =
			{
				"Product Name",
				"Supplier",
				"Category",
				"Quantity Per Unit",
				"Unit Price",
				"Units in Stock",
				"Units on Order",
				"Reorder Level",
				"Discontinued"
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
