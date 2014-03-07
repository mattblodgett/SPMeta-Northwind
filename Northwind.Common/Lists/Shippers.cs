using CamlexNET;
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

			EnsureViews(list);
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

		private void EnsureViews(SPList list)
		{
			SPView defaultView = list.Views.EnsureDefaultView(new[]
			{
				"LinkTitle",
				"Phone"
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
