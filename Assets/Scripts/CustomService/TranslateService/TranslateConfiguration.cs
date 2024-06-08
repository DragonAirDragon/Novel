using System.Collections;
using System.Collections.Generic;
using Naninovel;
using Sirenix.OdinInspector;
using UnityEngine;

[EditInProjectSettings]
public class TranslateConfiguration : Configuration
{
    public string yandexDictionaryApi;
    public string merriamWebsterDictionaryApi;
    [Title("Basic Settings")]
    public DataWords dataWords;
    [FolderPath]
    public string audioClipPath;
    public ScriptTranslationData scriptTranslationData;

}
