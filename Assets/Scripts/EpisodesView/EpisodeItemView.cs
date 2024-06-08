using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using Naninovel;
public enum StateEpisodeItem
{

    Progress,
    WaitClaiming,
    Claimed,
    Block
}


public class EpisodeItemView : MonoBehaviour
{

    [Title("State")]

    [OnValueChanged("ChangeEpisodeItemState")]
    public StateEpisodeItem currentStateEpisodeItem = StateEpisodeItem.Progress;


    [Title("Current Settings States")]
    public DataStatesEpisodes currentDataStatesEpisodes;

    [Title("Local Data")]
    [OnValueChanged("ChangeNameEpisode")]
    public string nameEpisode;
    [OnValueChanged("ChangeDescriptionEpisode")]
    public string descriptionEpisode;


    [OnValueChanged("ChangeProgressEpisode")]
    public float progressEpisode;



    [Title("Ref")]
    public Text nameText;
    public Text descriptionText;
    public Slider sliderProgress;
    public Image imageSlider;
    public Image backgroundImageSlider;
    public Text percentSliderText;

    public Image iconState;
    public Image splashImage;
    public Image backgroundImage;

    public Image blockImage;

    void Start()
    {


        ChangeEpisodeItemState();
        ChangeNameEpisode();
        ChangeDescriptionEpisode();
        ChangeProgressEpisode();
    }

    public void SetEpisodeItemParameters(string enterName, string enterDiscripion, StateEpisodeItem enterEpisodeState, Sprite enterSpriteSplash, float enterProgressEpisode)
    {
        nameEpisode = enterName;
        descriptionEpisode = enterDiscripion;
        currentStateEpisodeItem = enterEpisodeState;
        progressEpisode = enterProgressEpisode;
        splashImage.sprite = enterSpriteSplash;
    }


    //OnChangeMethods
    private void ChangeEpisodeItemState()
    {
        nameText.color = currentDataStatesEpisodes.DictionaryNameAndElementsStatesEpisodeItem[this.currentStateEpisodeItem].nameEpisodeColor;
        descriptionText.color = currentDataStatesEpisodes.DictionaryNameAndElementsStatesEpisodeItem[this.currentStateEpisodeItem].descriptionEpisodeColor;
        percentSliderText.color = currentDataStatesEpisodes.DictionaryNameAndElementsStatesEpisodeItem[this.currentStateEpisodeItem].percentTextColor;
        imageSlider.color = currentDataStatesEpisodes.DictionaryNameAndElementsStatesEpisodeItem[this.currentStateEpisodeItem].sliderColor;
        backgroundImageSlider.color = currentDataStatesEpisodes.DictionaryNameAndElementsStatesEpisodeItem[this.currentStateEpisodeItem].backgroundSliderColor;
        iconState.sprite = currentDataStatesEpisodes.DictionaryNameAndElementsStatesEpisodeItem[this.currentStateEpisodeItem].iconState;
        iconState.color = currentDataStatesEpisodes.DictionaryNameAndElementsStatesEpisodeItem[this.currentStateEpisodeItem].colorIconState;
        backgroundImage.color = currentDataStatesEpisodes.DictionaryNameAndElementsStatesEpisodeItem[this.currentStateEpisodeItem].colorBackground;
        if (currentStateEpisodeItem == StateEpisodeItem.Block)
        {
            blockImage.gameObject.SetActive(true);
            splashImage.gameObject.SetActive(false);
            GetComponent<Button>().interactable = false;
        }
        else
        {
            blockImage.gameObject.SetActive(false);
            splashImage.gameObject.SetActive(true);
            GetComponent<Button>().interactable = true;
        }


    }

    private void ChangeNameEpisode()
    {
        nameText.text = nameEpisode;
    }
    private void ChangeDescriptionEpisode()
    {
        descriptionText.text = descriptionEpisode;
    }
    private void ChangeProgressEpisode()
    {
        sliderProgress.value = progressEpisode;
        percentSliderText.text = ((int)(progressEpisode * 100)).ToString() + "%";
    }




}
