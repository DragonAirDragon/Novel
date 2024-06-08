using System;
using System.Collections.Generic;
using Naninovel;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptTransaltionData", menuName = "ScriptTransaltionData", order = 0)]
public class ScriptTranslationData : SerializedScriptableObject
{
    [NonSerialized, OdinSerialize, ShowInInspector]
    public Dictionary<String, String> scriptWithTranslation = new();
}
