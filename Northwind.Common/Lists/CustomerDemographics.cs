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

			EnsureFieldOrder(list);
		}

		private void EnsureFields(SPList list)
		{
			list.Fields.Ensure<SPFieldMultiLineText>("Customer Desc");
		}

		private void EnsureFieldOrder(SPList list)
		{
			string[] fieldOrder =
			{
				"ID",
				"Customer Desc"
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
