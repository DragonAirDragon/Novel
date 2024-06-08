using System;

using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;



[CreateAssetMenu(fileName = "DataStatesEpisodes", menuName = "DataStatesEpisodes", order = 0)]
public class DataStatesEpisodes : SerializedScriptableObject {
    
    [ShowInInspector,NonSerialized, OdinSerialize]
    public Dictionary<StateEpisodeItem,ElementsForStateEpisode> DictionaryNameAndElementsStatesEpisodeItem = new Dictionary<StateEpisodeItem, ElementsForStateEpisode>();

}
[Serializable]
public class ElementsForStateEpisode{
        public Sprite iconState;
        public Color colorIconState;
        public Color colorBackground;
        public Color nameEpisodeColor;
        public Color descriptionEpisodeColor;
        public Color backgroundSliderColor;
        public Color sliderColor;
        public Color percentTextColor;
}
