using System;
using System.Collections.Generic;
using Microsoft.SharePoint;
using Northwind.Common.Utilities;

namespace Northwind.Common.Extensions
{
	public static class SPFieldCollectionExtensions
	{
		public static T Ensure<T>(this SPFieldCollection fields, string fieldDisplayName) where T : SPField
		{
			if (typeof(T) == typeof(SPFieldLookup))
			{
				throw new ArgumentException("Lookups are not supported");
			}

			SPFieldType fieldType = SharePointHelper.GetFieldType<T>();

			if (fields.ContainsField(fieldDisplayName))
			{
				SPField field = fields.GetField(fieldDisplayName);

				if (field.Type == fieldType)
				{
					return field as T;
				}

				field.Delete();
			}

			fields.Add(fieldDisplayName, fieldType, false);

			return fields.GetField(fieldDisplayName) as T;
		}

		public static SPFieldLookup EnsureLookup(this SPFieldCollection fields, string lookupListTitle, string fieldDisplayName)
		{
			SPList lookupList = fields.Web.Lists.TryGetList(lookupListTitle);

			if (lookupList == null)
			{
				throw new ArgumentException("Lookup list does not exist: " + lookupListTitle);
			}

			if (fields.ContainsField(fieldDisplayName))
			{
				SPField field = fields.GetField(fieldDisplayName);

				if (field.Type == SPFieldType.Lookup)
				{
					return field as SPFieldLookup;
				}

				field.Delete();
			}

			fields.AddLookup(fieldDisplayName, lookupList.ID, false);

			return fields.GetField(fieldDisplayName) as SPFieldLookup;
		}

		public static void Reorder(this SPFieldCollection fields, params string[] fieldNames)
		{
			SPContentType itemContentType = fields.List.ContentTypes["Item"];

			List<string> fieldInternalNames = new List<string>();

			foreach (string fieldName in fieldNames)
			{
				if (itemContentType.Fields.ContainsField(fieldName))
				{
					fieldInternalNames.Add(itemContentType.Fields[fieldName].InternalName);
				}
			}

			itemContentType.FieldLinks.Reorder(fieldInternalNames.ToArray());

			itemContentType.Update();
		}

		public static void SuppressTitle(this SPFieldCollection fields)
		{
			SPField title = fields.GetField("Title");
			title.Hidden = true;
			title.Required = false;
			title.Update();
		}
	}
}