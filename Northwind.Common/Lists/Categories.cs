using CamlexNET;
using Microsoft.SharePoint;
using Northwind.Common.Extensions;
using Northwind.Common.Lists.Base;

namespace Northwind.Common.Lists
{
	public class Categories : IEnsurableList
	{
		private const string LIST_TITLE = "Categories";

		public void Ensure(SPWeb web)
		{
			SPList list = web.Lists.Ensure(SPListTemplateType.GenericList, LIST_TITLE);

			EnsureFields(list);

			EnsureViews(list);
		}

		private void EnsureFields(SPList list)
		{
			SPFieldText categoryName = list.Fields[SPBuiltInFieldId.Title] as SPFieldText;
			categoryName.Title = "Category Name";
			categoryName.MaxLength = 15;
			categoryName.Required = true;
			categoryName.Update();

			list.Fields.Ensure<SPFieldMultiLineText>("Description");

			SPFieldUrl picture = list.Fields.Ensure<SPFieldUrl>("Picture");
			picture.DisplayFormat = SPUrlFieldFormatType.Image;
			picture.Update();

			list.Fields.Reorder(new[]
			{
				"Category Name",
				"Description",
				"Picture"
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
