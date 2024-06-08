
using System.Collections.Generic;
using System.Linq;
using Naninovel;
using Naninovel.UI;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class EpisodesManager : MonoBehaviour
{
    [Title("Episodes")]
    public List<EpisodeItemView> currentEpisodes = new List<EpisodeItemView>();
    EpisodeService episodeService;
    public EpisodeItemView exampleEpisodeItem;

    [Title("Ref")]
    public Transform parentEpsiodes;
    public Button claimAllButton;
    public Button resetSeasonButton;
    public EpisodesMenu episodesMenu;




    private void EpisodeButton(int i)
    {
        var uiManager = Engine.GetService<IUIManager>();
        uiManager.GetUI<EpisodeMenu>()?.Show();
        uiManager.GetUI<EpisodeMenu>()?.SetEpisodeInfoAndUpdateUI(i);
    }
    private void Awake()
    {
        episodeService = Engine.GetService<EpisodeService>();
        claimAllButton.onClick.AddListener(episodeService.ClaimAllEpisodes);
        episodeService.OnEpisodesChanges += UpdateUI;
    }


    /// <summary>
    /// Update UI
    /// </summary>
    public void UpdateUI()
    {
        //Delete local episodes
        foreach (var currentEpisode in currentEpisodes)
        {
            Destroy(currentEpisode?.gameObject);
        }
        currentEpisodes.Clear();

        //Load episodes from current data
        foreach (var currentEpisodeInfo in episodeService.GetEpisodes().Select((value, i) => (value, i)))
        {
            currentEpisodes.Add(Instantiate(exampleEpisodeItem, parentEpsiodes));
            float currentProgress = currentEpisodeInfo.value.currentProgressValue;
            float progressInFloat = currentProgress / currentEpisodeInfo.value.maxProgressValue;
            Debug.Log(progressInFloat);
            currentEpisodes.Last().SetEpisodeItemParameters(
                currentEpisodeInfo.value.epsiodeName,
                currentEpisodeInfo.value.description,
                currentEpisodeInfo.value.currentEpisodeState,
                currentEpisodeInfo.value.splashImage,
                progressInFloat
            );

            currentEpisodes.Last().GetComponent<Button>().onClick.AddListener(() =>
            {
                EpisodeButton(currentEpisodeInfo.i);
            });
        }

        CheckStateAndSetVisibleButton();
        CheckCostAllEpisodesAndSetInteractiveButtonResetSeason();

    }
    // Claims
    private void CheckStateAndSetVisibleButton()
    {
        claimAllButton.gameObject.SetActive(episodeService.CheckStatesThatWaitClimbing());
    }

    public void CheckCostAllEpisodesAndSetInteractiveButtonResetSeason()
    {
        resetSeasonButton.interactable = episodeService.CanAffordRestartSeason();
    }
}
