using CamlexNET;
using Microsoft.SharePoint;
using Northwind.Common.Extensions;
using Northwind.Common.Lists.Base;

namespace Northwind.Common.Lists
{
	public class Employees : IEnsurableList
	{
		private const string LIST_TITLE = "Employees";

		public void Ensure(SPWeb web)
		{
			SPList list = web.Lists.Ensure(SPListTemplateType.GenericList, LIST_TITLE);

			EnsureFields(list);

			EnsureViews(list);
		}

		private void EnsureFields(SPList list)
		{
			SPFieldText lastName = list.Fields.Ensure<SPFieldText>("Last Name");
			lastName.Required = true;
			lastName.MaxLength = 20;
			lastName.Update();

			SPFieldText firstName = list.Fields.Ensure<SPFieldText>("First Name");
			firstName.Required = true;
			firstName.MaxLength = 10;
			firstName.Update();

			SPFieldText title = list.Fields.Ensure<SPFieldText>("Title");
			title.Required = false;
			title.MaxLength = 30;
			title.Update();

			SPFieldText titleOfCourtesy = list.Fields.Ensure<SPFieldText>("Title of Courtesy");
			titleOfCourtesy.MaxLength = 25;
			titleOfCourtesy.Update();

			SPFieldDateTime birthDate = list.Fields.Ensure<SPFieldDateTime>("Birth Date");
			birthDate.DisplayFormat = SPDateTimeFieldFormatType.DateOnly;
			birthDate.Update();

			SPFieldDateTime hireDate = list.Fields.Ensure<SPFieldDateTime>("Hire Date");
			hireDate.DisplayFormat = SPDateTimeFieldFormatType.DateOnly;
			hireDate.Update();

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

			SPFieldText homePhone = list.Fields.Ensure<SPFieldText>("Home Phone");
			homePhone.MaxLength = 24;
			homePhone.Update();

			SPFieldText extension = list.Fields.Ensure<SPFieldText>("Extension");
			extension.MaxLength = 4;
			extension.Update();

			SPFieldUrl photo = list.Fields.Ensure<SPFieldUrl>("Photo");
			photo.DisplayFormat = SPUrlFieldFormatType.Image;
			photo.Update();

			list.Fields.Ensure<SPFieldMultiLineText>("Notes");

			SPFieldLookup territories = list.Fields.EnsureLookup("Territories", "Territories");
			territories.AllowMultipleValues = true;
			territories.Update();

			SPFieldCalculated displayName = list.Fields.Ensure<SPFieldCalculated>("Display Name");
			displayName.Formula = "=[First Name]&\" \"&[Last Name]";
			displayName.Update();

			SPFieldLookup reportsTo = list.Fields.EnsureLookup(LIST_TITLE, "Reports To");
			reportsTo.LookupField = displayName.InternalName;
			reportsTo.Update();
		}

		private void EnsureViews(SPList list)
		{
			SPView defaultView = list.Views.EnsureDefaultView(new[]
			{
				"First Name",
				"Last Name",
				"Title",
				"Reports To",
				"City",
				"Region",
				"Country"
			});

			string lastNameInternal = list.Fields["Last Name"].InternalName;
			defaultView.Query = Camlex.Query().OrderBy(x => x[lastNameInternal]).ToString();
			defaultView.Update();
		}

		public void TearDown(SPWeb web)
		{
			web.Lists.Delete(LIST_TITLE);
		}
	}
}
