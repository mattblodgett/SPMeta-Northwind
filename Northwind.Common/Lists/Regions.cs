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

			EnsureFieldOrder(list);
		}

		private void EnsureFields(SPList list)
		{
			SPFieldText regionDescription = list.Fields[SPBuiltInFieldId.Title] as SPFieldText;
			regionDescription.Title = "Region Description";
			regionDescription.MaxLength = 50;
			regionDescription.Required = true;
			regionDescription.Update();
		}

		private void EnsureFieldOrder(SPList list)
		{
			string[] fieldOrder =
			{
				"ID",
				"Region Description"
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
