using System;
using Microsoft.SharePoint;

namespace Northwind.Common.Utilities
{
	public class SharePointHelper
	{
		public static SPFieldType GetFieldType<T>() where T : SPField
		{
			Type type = typeof(T);

			if (type == typeof(SPFieldBoolean))
			{
				return SPFieldType.Boolean;
			}

			if (type == typeof(SPFieldCalculated))
			{
				return SPFieldType.Calculated;
			}

			if (type == typeof(SPFieldDateTime))
			{
				return SPFieldType.DateTime;
			}

			if (type == typeof(SPFieldLookup))
			{
				return SPFieldType.Lookup;
			}

			if (type == typeof(SPFieldUser))
			{
				return SPFieldType.User;
			}

			if (type == typeof(SPFieldMultiChoice))
			{
				return SPFieldType.MultiChoice;
			}

			if (type == typeof(SPFieldChoice))
			{
				return SPFieldType.Choice;
			}

			if (type == typeof(SPFieldWorkflowStatus))
			{
				return SPFieldType.WorkflowStatus;
			}

			if (type == typeof(SPFieldMultiLineText))
			{
				return SPFieldType.Note;
			}

			if (type == typeof(SPFieldNumber))
			{
				return SPFieldType.Number;
			}

			if (type == typeof(SPFieldCurrency))
			{
				return SPFieldType.Currency;
			}

			if (type == typeof(SPFieldText))
			{
				return SPFieldType.Text;
			}

			if (type == typeof(SPFieldUrl))
			{
				return SPFieldType.URL;
			}

			throw new ArgumentException("No mapping implemented for type: " + type);
		}
	}
}
