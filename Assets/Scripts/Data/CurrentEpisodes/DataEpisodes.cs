using System;
using System.Collections;
using System.Collections.Generic;
using Naninovel;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

[CreateAssetMenu(fileName = "DataEpisodes", menuName = "DataEpisodes", order = 0)]
public class DataEpisodes : SerializedScriptableObject
{
    [ShowInInspector, NonSerialized, OdinSerialize]
    public List<EpisodeInfo> currentEpisodes = new List<EpisodeInfo>();
}

[Serializable]
public class EpisodeInfo
{
    public string epsiodeName;
    public string description;

    public string fullDescription;
    public StateEpisodeItem currentEpisodeState;
    public string saveName;
    public string nameProgressVariable;

    public string nameScript;
    public int currentProgressValue;
    public int maxProgressValue;
    public Sprite splashImage;
    public int rewardsCrystal;

    public int energyCost;

    public int resetCrystal;

    public bool unblockable;

    public Sprite fullSplashImage;
}
