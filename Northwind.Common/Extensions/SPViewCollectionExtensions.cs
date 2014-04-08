using System;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.SharePoint;

namespace Northwind.Common.Extensions
{
	public static class SPViewCollectionExtensions
	{
		public static SPView EnsureDefaultView(this SPViewCollection spViewCollection, string[] viewFields)
		{
			spViewCollection.Delete(spViewCollection.List.DefaultView.ID);

			StringCollection viewFieldsCol = new StringCollection();
			viewFieldsCol.AddRange(viewFields);

			return spViewCollection.Add("All", viewFieldsCol, string.Empty, 100, true, true);
		}

		public static SPView EnsureView(this SPViewCollection spViewCollection, string viewName, string[] viewFields)
		{
			SPView view = spViewCollection.GetView(viewName);

			StringCollection scViewFields = new StringCollection();
			scViewFields.AddRange(viewFields);

			if (view != null)
			{
				view.ViewFields.DeleteAll();
				viewFields.ToList().ForEach(f => view.ViewFields.Add(f));
				view.Update();

				view = spViewCollection[viewName];
			}
			else
			{
				view = spViewCollection.Add(viewName, scViewFields, string.Empty, 30, true, false);
			}

			return view;
		}

		public static SPView GetView(this SPViewCollection spViewCollection, string viewName)
		{
			SPView view = null;

			try
			{
				view = spViewCollection[viewName];
			}
			catch (ArgumentException)
			{
				// view does not exist
			}

			return view;
		}
	}
}
