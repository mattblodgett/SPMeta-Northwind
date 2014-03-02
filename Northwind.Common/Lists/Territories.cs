using Microsoft.SharePoint;
using Northwind.Common.Extensions;
using Northwind.Common.Lists.Base;

namespace Northwind.Common.Lists
{
	public class Territories : IEnsurableList
	{
		private const string LIST_TITLE = "Territories";

		public void Ensure(SPWeb web)
		{
			SPList list = web.Lists.Ensure(SPListTemplateType.GenericList, LIST_TITLE);

			EnsureFields(list);

			EnsureFieldOrder(list);
		}

		private void EnsureFields(SPList list)
		{
			SPFieldText territoryDescription = list.Fields[SPBuiltInFieldId.Title] as SPFieldText;
			territoryDescription.Title = "Territory Description";
			territoryDescription.MaxLength = 50;
			territoryDescription.Required = true;
			territoryDescription.Update();

			SPFieldLookup region = list.Fields.EnsureLookup("Regions", "Region");
			region.Required = true;
			region.Update();
		}

		private void EnsureFieldOrder(SPList list)
		{
			string[] fieldOrder =
			{
				"ID",
				"Territory Description",
				"Region"
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
