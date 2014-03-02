using Microsoft.SharePoint;

namespace Northwind.Common.Lists.Base
{
	public interface IEnsurableList
	{
		void Ensure(SPWeb web);
		void TearDown(SPWeb web);
	}
}