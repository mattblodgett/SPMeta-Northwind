using System;
using Microsoft.SharePoint;

namespace Northwind.Common.Extensions
{
	public static class SPListCollectionExtensions
	{
		public static SPList Ensure(this SPListCollection lists, SPListTemplateType listType, string listTitle)
		{
			SPList list = lists.TryGetList(listTitle);

			if (list == null)
			{
				Guid listId = lists.Add(listTitle, string.Empty, listType);
				list = lists[listId];
			}

			return list;
		}

		public static void Delete(this SPListCollection lists, string listTitle)
		{
			SPList list = lists.TryGetList(listTitle);

			if (list != null)
			{
				list.Delete();
			}
		}
	}
}