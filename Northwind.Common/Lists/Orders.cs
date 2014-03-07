using CamlexNET;
using Microsoft.SharePoint;
using Northwind.Common.Extensions;
using Northwind.Common.Lists.Base;

namespace Northwind.Common.Lists
{
	public class Orders : IEnsurableList
	{
		private const string LIST_TITLE = "Orders";

		public void Ensure(SPWeb web)
		{
			SPList list = web.Lists.Ensure(SPListTemplateType.GenericList, LIST_TITLE);

			EnsureFields(list);

			EnsureViews(list);
		}

		private void EnsureFields(SPList list)
		{
			list.Fields.SuppressTitle();

			SPList customers = list.Lists["Customers"];
			SPFieldLookup customer = list.Fields.EnsureLookup(customers.Title, "Customer");
			customer.LookupField = customers.Fields["Company Name"].InternalName;
			customer.Update();

			SPList employees = list.Lists["Employees"];
			SPFieldLookup employee = list.Fields.EnsureLookup(employees.Title, "Employee");
			employee.LookupField = employees.Fields["Display Name"].InternalName;
			employee.Update();

			SPFieldDateTime orderDate = list.Fields.Ensure<SPFieldDateTime>("Order Date");
			orderDate.DisplayFormat = SPDateTimeFieldFormatType.DateTime;
			orderDate.DefaultValue = "[today]";
			orderDate.Update();

			SPFieldDateTime requiredDate = list.Fields.Ensure<SPFieldDateTime>("Required Date");
			requiredDate.DisplayFormat = SPDateTimeFieldFormatType.DateTime;
			requiredDate.Update();

			SPFieldDateTime shippedDate = list.Fields.Ensure<SPFieldDateTime>("Shipped Date");
			shippedDate.DisplayFormat = SPDateTimeFieldFormatType.DateTime;
			shippedDate.Update();

			SPList shippers = list.Lists["Shippers"];
			SPFieldLookup shipVia = list.Fields.EnsureLookup(shippers.Title, "Ship Via");
			shipVia.LookupField = shippers.Fields["Company Name"].InternalName;
			shipVia.Update();

			SPFieldCurrency freight = list.Fields.Ensure<SPFieldCurrency>("Freight");
			freight.MinimumValue = 0;
			freight.Update();

			SPFieldText shipName = list.Fields.Ensure<SPFieldText>("Ship Name");
			shipName.MaxLength = 40;
			shipName.Update();

			SPFieldText shipAddress = list.Fields.Ensure<SPFieldText>("Ship Address");
			shipAddress.MaxLength = 60;
			shipAddress.Update();

			SPFieldText shipCity = list.Fields.Ensure<SPFieldText>("Ship City");
			shipCity.MaxLength = 15;
			shipCity.Update();

			SPFieldText shipRegion = list.Fields.Ensure<SPFieldText>("Ship Region");
			shipRegion.MaxLength = 15;
			shipRegion.Update();

			SPFieldText shipPostalCode = list.Fields.Ensure<SPFieldText>("Ship Postal Code");
			shipPostalCode.MaxLength = 10;
			shipPostalCode.Update();

			SPFieldText shipCountry = list.Fields.Ensure<SPFieldText>("Ship Country");
			shipCountry.MaxLength = 15;
			shipCountry.Update();
		}

		private void EnsureViews(SPList list)
		{
			SPView defaultView = list.Views.EnsureDefaultView(new[]
			{
				"Order Date",
				"Customer",
				"Required Date",
				"Shipped Date"
			});

			string orderDateInternal = list.Fields["Order Date"].InternalName;
			defaultView.Query = Camlex.Query().OrderBy(x => x[orderDateInternal] as Camlex.Desc).ToString();
			defaultView.Update();
		}

		public void TearDown(SPWeb web)
		{
			web.Lists.Delete(LIST_TITLE);
		}
	}
}
