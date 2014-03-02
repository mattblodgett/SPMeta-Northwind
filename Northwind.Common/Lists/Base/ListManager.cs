using System.Collections.Generic;
using Microsoft.SharePoint;

namespace Northwind.Common.Lists.Base
{
	public class ListManager
	{
		private readonly List<IEnsurableList> ensurableLists;

		public ListManager(List<IEnsurableList> ensurableLists)
		{
			this.ensurableLists = ensurableLists;
		}

		public void EnsureLists(SPWeb web)
		{
			foreach (IEnsurableList list in ensurableLists)
			{
				list.Ensure(web);
			}
		}

		public void TearDownLists(SPWeb web)
		{
			ensurableLists.Reverse();

			foreach (IEnsurableList list in ensurableLists)
			{
				list.TearDown(web);
			}

			ensurableLists.Reverse();
		}
	}
}