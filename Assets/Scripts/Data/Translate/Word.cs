using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
[Serializable]
public class Word
{
    public string englishWord;
    public string russianWord;
    public string partOfSpeech;
    public AudioClip audioClip;
    [FilePath]
    public string pathAudio;

    [Title("Info For SuperMemo")]
    public int repetition = 0;
    public float easinessFactor = 2.5f;
    public int interval = 1;
    public DateTime nextReviewDate = DateTime.Now;
}