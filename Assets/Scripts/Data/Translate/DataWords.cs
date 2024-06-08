using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
[CreateAssetMenu(fileName = "Words", menuName = "Words", order = 0)]
public class DataWords : SerializedScriptableObject
{

    [NonSerialized, OdinSerialize]
    public List<Word> words = new List<Word>();
    public void RemoveWord(Word word)
    {
        words.Remove(word);
    }
    public void AddWord(Word word)
    {
        if (!words.Contains(word))
        {
            words.Add(word);
        }
    }
}

