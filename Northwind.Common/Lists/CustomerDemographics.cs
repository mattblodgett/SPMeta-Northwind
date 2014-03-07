using CamlexNET;
using Microsoft.SharePoint;
using Northwind.Common.Extensions;
using Northwind.Common.Lists.Base;

namespace Northwind.Common.Lists
{
	public class CustomerDemographics : IEnsurableList
	{
		private const string LIST_TITLE = "Customer Demographics";

		public void Ensure(SPWeb web)
		{
			SPList list = web.Lists.Ensure(SPListTemplateType.GenericList, LIST_TITLE);

			EnsureFields(list);

			EnsureViews(list);
		}

		private void EnsureFields(SPList list)
		{
			SPField demographic = list.Fields[SPBuiltInFieldId.Title];
			demographic.Title = "Demographic";
			demographic.Indexed = true;
			demographic.EnforceUniqueValues = true;
			demographic.Update();

			list.Fields.Ensure<SPFieldMultiLineText>("Description");

			list.Fields.Reorder(new[]
			{
				"Demographic",
				"Description"
			});
		}

		private void EnsureViews(SPList list)
		{
			SPView defaultView = list.Views.EnsureDefaultView(new[]
			{
				"LinkTitle",
				"Description"
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
