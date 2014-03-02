using Microsoft.SharePoint;
using Northwind.Common.Extensions;
using Northwind.Common.Lists.Base;

namespace Northwind.Common.Lists
{
	public class Customers : IEnsurableList
	{
		private const string LIST_TITLE = "Customers";

		public void Ensure(SPWeb web)
		{
			SPList list = web.Lists.Ensure(SPListTemplateType.GenericList, LIST_TITLE);

			EnsureFields(list);

			EnsureFieldOrder(list);
		}

		private void EnsureFields(SPList list)
		{
			SPFieldText customerId = list.Fields.Ensure<SPFieldText>("Customer ID");
			customerId.Required = true;
			customerId.Indexed = true;
			customerId.EnforceUniqueValues = true;
			customerId.MaxLength = 5;
			customerId.ValidationFormula = "=LEN([Customer ID])=5";
			customerId.ValidationMessage = "Customer ID must be exactly 5 characters.";
			customerId.Update();

			SPFieldText companyName = list.Fields[SPBuiltInFieldId.Title] as SPFieldText;
			companyName.Title = "Company Name";
			companyName.Required = true;
			companyName.MaxLength = 40;
			companyName.Update();

			SPFieldText contactName = list.Fields.Ensure<SPFieldText>("Contact Name");
			contactName.MaxLength = 30;
			contactName.Update();

			SPFieldText contactTitle = list.Fields.Ensure<SPFieldText>("Contact Title");
			contactTitle.MaxLength = 30;
			contactTitle.Update();

			SPFieldText address = list.Fields.Ensure<SPFieldText>("Address");
			address.MaxLength = 60;
			address.Update();

			SPFieldText city = list.Fields.Ensure<SPFieldText>("City");
			city.MaxLength = 15;
			city.Update();

			SPFieldText region = list.Fields.Ensure<SPFieldText>("Region");
			region.MaxLength = 15;
			region.Update();

			SPFieldText postalCode = list.Fields.Ensure<SPFieldText>("Postal Code");
			postalCode.MaxLength = 10;
			postalCode.Update();

			SPFieldText country = list.Fields.Ensure<SPFieldText>("Country");
			country.MaxLength = 15;
			country.Update();

			SPFieldText phone = list.Fields.Ensure<SPFieldText>("Phone");
			phone.MaxLength = 24;
			phone.Update();

			SPFieldText fax = list.Fields.Ensure<SPFieldText>("Fax");
			fax.MaxLength = 24;
			fax.Update();

			SPFieldLookup customerDemographics = list.Fields.EnsureLookup("Customer Demographics", "Customer Demographics");
			customerDemographics.AllowMultipleValues = true;
			customerDemographics.Update();
		}

		private void EnsureFieldOrder(SPList list)
		{
			string[] fieldOrder =
			{
				"Customer ID",
				"Company Name",
				"Contact Name",
				"Contact Title",
				"Address",
				"City",
				"Region",
				"Postal Code",
				"Country",
				"Phone",
				"Fax",
				"Customer Demographics"
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
