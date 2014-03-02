using Microsoft.SharePoint;
using Northwind.Common.Extensions;
using Northwind.Common.Lists.Base;

namespace Northwind.Common.Lists
{
	public class EmployeeTerritories : IEnsurableList
	{
		private const string LIST_TITLE = "Employee Territories";

		public void Ensure(SPWeb web)
		{
			SPList list = web.Lists.Ensure(SPListTemplateType.GenericList, LIST_TITLE);

			EnsureFields(list);

			EnsureFieldOrder(list);
		}

		private void EnsureFields(SPList list)
		{
			list.Fields.SuppressTitle();

			SPList employees = list.Lists["Employees"];
			SPFieldLookup employee = list.Fields.EnsureLookup(employees.Title, "Employee");
			employee.LookupField = employees.Fields["Last Name"].InternalName;
			employee.Required = true;
			employee.Indexed = true;
			employee.RelationshipDeleteBehavior = SPRelationshipDeleteBehavior.Cascade;
			employee.Update();

			SPFieldLookup territory = list.Fields.EnsureLookup("Territories", "Territory");
			territory.Required = true;
			territory.Indexed = true;
			territory.RelationshipDeleteBehavior = SPRelationshipDeleteBehavior.Cascade;
			territory.Update();
		}

		private void EnsureFieldOrder(SPList list)
		{
			string[] fieldOrder =
			{
				"Employee",
				"Territory"
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
