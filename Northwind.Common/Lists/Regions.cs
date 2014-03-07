using CamlexNET;
using Microsoft.SharePoint;
using Northwind.Common.Extensions;
using Northwind.Common.Lists.Base;

namespace Northwind.Common.Lists
{
	public class Regions : IEnsurableList
	{
		private const string LIST_TITLE = "Regions";

		public void Ensure(SPWeb web)
		{
			SPList list = web.Lists.Ensure(SPListTemplateType.GenericList, LIST_TITLE);

			EnsureFields(list);

			EnsureViews(list);
		}

		private void EnsureFields(SPList list)
		{
			SPFieldText regionName = list.Fields[SPBuiltInFieldId.Title] as SPFieldText;
			regionName.Title = "Region Name";
			regionName.Indexed = true;
			regionName.EnforceUniqueValues = true;
			regionName.Required = true;
			regionName.Update();
		}

		private void EnsureViews(SPList list)
		{
			SPView defaultView = list.Views.EnsureDefaultView(new[]
			{
				"LinkTitle"
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
