using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x0200219F RID: 8607
	internal static class CustomLocalization
	{
		// Token: 0x0600F36F RID: 62319 RVA: 0x004ACAC8 File Offset: 0x004AACC8
		public static bool TryGet(string term, out string value)
		{
			value = null;
			if (!CustomLocalization.loaded)
			{
				CustomLocalization.Load();
			}
			else if (CustomLocalization.HasFileChanged())
			{
				CustomLocalization.Load();
			}
			if (CustomLocalization.dict == null)
			{
				return false;
			}
			if (CustomLocalization.dict.TryGetValue(term, out value))
			{
				return true;
			}
			if (!CustomLocalization.missingTerms.Contains(term))
			{
				CustomLocalization.missingTerms.Add(term);
			}
			return false;
		}

		// Token: 0x0600F370 RID: 62320 RVA: 0x004ACB28 File Offset: 0x004AAD28
		private static void Load()
		{
			CustomLocalization.loaded = true;
			CustomLocalization.dict = null;
			string path = Path.Combine(Application.dataPath, "../CustomLang/lang.json");
			if (!File.Exists(path))
			{
				CustomLocalization.lastWriteTime = DateTime.MinValue;
				return;
			}
			try
			{
				CustomLocalization.lastWriteTime = File.GetLastWriteTime(path);
				CustomLocalization.Wrapper wrapper = JsonUtility.FromJson<CustomLocalization.Wrapper>(File.ReadAllText(path).Replace('ı', 'i').Replace('İ', 'I').Replace('ğ', 'g').Replace('Ğ', 'G'));
				if (wrapper != null)
				{
					CustomLocalization.dict = wrapper.ToDict();
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600F372 RID: 62322 RVA: 0x004ACBD0 File Offset: 0x004AADD0
		private static bool HasFileChanged()
		{
			string path = Path.Combine(Application.dataPath, "../CustomLang/lang.json");
			if (!File.Exists(path))
			{
				return false;
			}
			bool result;
			try
			{
				result = (File.GetLastWriteTime(path) != CustomLocalization.lastWriteTime);
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0400C301 RID: 49921
		private static Dictionary<string, string> dict;

		// Token: 0x0400C302 RID: 49922
		private static bool loaded;

		// Token: 0x0400C303 RID: 49923
		private static bool missingLogged;

		// Token: 0x0400C304 RID: 49924
		private static HashSet<string> missingTerms = new HashSet<string>();

		// Token: 0x0400C305 RID: 49925
		private static DateTime lastWriteTime = DateTime.MinValue;

		// Token: 0x020021A0 RID: 8608
		[Serializable]
		private class Wrapper
		{
			// Token: 0x0600F373 RID: 62323 RVA: 0x004ACC24 File Offset: 0x004AAE24
			public Dictionary<string, string> ToDict()
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				if (this.entries == null)
				{
					return dictionary;
				}
				foreach (CustomLocalization.Entry entry in this.entries)
				{
					if (!string.IsNullOrEmpty(entry.key))
					{
						dictionary[entry.key] = (entry.value ?? string.Empty);
					}
				}
				return dictionary;
			}

			// Token: 0x0400C306 RID: 49926
			public List<CustomLocalization.Entry> entries;
		}

		// Token: 0x020021A1 RID: 8609
		[Serializable]
		private class Entry
		{
			// Token: 0x0400C307 RID: 49927
			public string key;

			// Token: 0x0400C308 RID: 49928
			public string value;
		}
	}
}
