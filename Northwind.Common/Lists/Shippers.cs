using Microsoft.SharePoint;
using Northwind.Common.Extensions;
using Northwind.Common.Lists.Base;

namespace Northwind.Common.Lists
{
	public class Shippers : IEnsurableList
	{
		private const string LIST_TITLE = "Shippers";

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

			SPFieldText phone = list.Fields.Ensure<SPFieldText>("Phone");
			phone.MaxLength = 24;
			phone.Update();
		}

		private void EnsureFieldOrder(SPList list)
		{
			string[] fieldOrder =
			{
				"Company Name",
				"Phone"
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
