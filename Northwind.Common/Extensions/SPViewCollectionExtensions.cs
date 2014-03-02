using System.Collections.Specialized;
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
	}
}
