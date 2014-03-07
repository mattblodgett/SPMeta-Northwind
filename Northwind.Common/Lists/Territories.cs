using CamlexNET;
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

			EnsureViews(list);
		}

		private void EnsureFields(SPList list)
		{
			SPFieldText territoryName = list.Fields[SPBuiltInFieldId.Title] as SPFieldText;
			territoryName.Title = "Territory Name";
			territoryName.Indexed = true;
			territoryName.EnforceUniqueValues = true;
			territoryName.Required = true;
			territoryName.Update();

			SPFieldLookup region = list.Fields.EnsureLookup("Regions", "Region");
			region.Required = true;
			region.Update();
		}

		private void EnsureViews(SPList list)
		{
			SPView defaultView = list.Views.EnsureDefaultView(new[]
			{
				"LinkTitle",
				"Region"
			});

			defaultView.Query = Camlex.Query().OrderBy(x => x["Title"]).ToString();
			defaultView.Update();
		}

		public void TearDown(SPWeb web)
		{
			web.Lists.Delete(LIST_TITLE);
		}
	}
}
