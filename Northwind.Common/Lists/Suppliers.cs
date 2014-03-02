using Microsoft.SharePoint;
using Northwind.Common.Extensions;
using Northwind.Common.Lists.Base;

namespace Northwind.Common.Lists
{
	public class Suppliers : IEnsurableList
	{
		private const string LIST_TITLE = "Suppliers";

		public void Ensure(SPWeb web)
		{
			SPList list = web.Lists.Ensure(SPListTemplateType.GenericList, LIST_TITLE);

			EnsureFields(list);

			EnsureFieldOrder(list);
		}

		private void EnsureFields(SPList list)
		{
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

			SPFieldMultiLineText homePage = list.Fields.Ensure<SPFieldMultiLineText>("Home Page");
		}

		private void EnsureFieldOrder(SPList list)
		{
			string[] fieldOrder =
			{
				"ID",
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
				"Home Page"
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
