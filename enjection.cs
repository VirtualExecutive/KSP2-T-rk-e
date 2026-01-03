using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace I2.Loc
{
	// Token: 0x02000D77 RID: 3447
	[ExecuteInEditMode]
	[Serializable]
	public partial class LanguageSourceData
	{
		// Token: 0x06006494 RID: 25748 RVA: 0x00282708 File Offset: 0x00280908
		public bool TryGetTranslation(string term, out string Translation, string overrideLanguage = null, string overrideSpecialization = null, bool skipDisabled = false, bool allowCategoryMistmatch = false)
		{
            // CHANGED
			if (CustomLocalization.TryGet(term, out Translation))
			{
				return true;
			}
            // CHANGED
			int languageIndex = this.GetLanguageIndex((overrideLanguage == null) ? LocalizationManager.CurrentLanguage : overrideLanguage, true, false);
			if (languageIndex >= 0 && (!skipDisabled || this.mLanguages[languageIndex].IsEnabled()))
			{
				TermData termData = this.GetTermData(term, allowCategoryMistmatch);
				if (termData != null)
				{
					Translation = termData.GetTranslation(languageIndex, overrideSpecialization, true);
					if (Translation == "---")
					{
						Translation = string.Empty;
						return true;
					}
					if (!string.IsNullOrEmpty(Translation))
					{
						return true;
					}
					Translation = null;
				}
				if (this.OnMissingTranslation == LanguageSourceData.MissingTranslationAction.ShowWarning)
				{
					Translation = "<!-Missing Translation [" + term + "]-!>";
					return true;
				}
				if (this.OnMissingTranslation == LanguageSourceData.MissingTranslationAction.Fallback && termData != null)
				{
					return this.TryGetFallbackTranslation(termData, out Translation, languageIndex, overrideSpecialization, skipDisabled);
				}
				if (this.OnMissingTranslation == LanguageSourceData.MissingTranslationAction.Empty)
				{
					Translation = string.Empty;
					return true;
				}
				if (this.OnMissingTranslation == LanguageSourceData.MissingTranslationAction.ShowTerm)
				{
					Translation = term;
					return true;
				}
			}
			Translation = null;
			return false;
		}
	}
}
