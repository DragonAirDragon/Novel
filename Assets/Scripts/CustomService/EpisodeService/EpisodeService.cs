using System;
using System.Collections;
using System.Collections.Generic;
using Naninovel;
using UnityEngine;
[InitializeAtRuntime(40)]
public class EpisodeService : IEngineService<EpisodeConfiguration>
{
    public EpisodeConfiguration Configuration { get; }
    public event Action OnEpisodesChanges;
    private IScriptManager scriptManager;
    private IStateManager stateManager;
    private ICustomVariableManager customVariableManager;

    private IScriptPlayer scriptPlayer;

    public EpisodeService(EpisodeConfiguration config, ICustomVariableManager customVariableManager, IScriptManager scriptManager, IStateManager stateManager, IScriptPlayer scriptPlayer)
    {
        Configuration = config;
        this.customVariableManager = customVariableManager;
        this.scriptManager = scriptManager;
        this.stateManager = stateManager;
        this.scriptPlayer = scriptPlayer;
    }

    public void DestroyService()
    {

    }

    public UniTask InitializeService()
    {
        SetNaninovelVariable();

        if (customVariableManager != null)
        {
            customVariableManager.OnVariableUpdated += OnUpdateVariables;
        }

        return UniTask.CompletedTask;
    }




    public void ResetService()
    {

    }

    /// <summary>
    /// Load saved variable and set value to variable NaniNovel
    /// </summary>
    public void SetNaninovelVariable()
    {
        foreach (var currentEpisode in Configuration.currentDataEpisodes.currentEpisodes)
        {
            customVariableManager.SetVariableValue(currentEpisode.nameProgressVariable, currentEpisode.currentProgressValue.ToString());
            CheckAndChangeStateEpisode();
        }
    }

    /// <summary>
    /// Invokes when variable NaniNovel Update
    /// Saves it local
    /// Change State Episodes (Progress=>WaitClimbing)
    /// </summary>
    /// <param name="customVariableUpdatedArgs">Updated VariableNaniNovel</param>
    public void OnUpdateVariables(CustomVariableUpdatedArgs customVariableUpdatedArgs)
    {
        SaveVariableLocal(customVariableUpdatedArgs);
        CheckAndChangeStateEpisode();
        OnEpisodesChanges?.Invoke();
    }




    public void SaveVariableLocal(CustomVariableUpdatedArgs customVariableUpdatedArgs)
    {
        foreach (var currentEpisode in Configuration.currentDataEpisodes.currentEpisodes)
        {
            if (currentEpisode.nameProgressVariable == customVariableUpdatedArgs.Name)
            {
                Debug.Log("Меняем значение" + currentEpisode.currentProgressValue.ToString() + "на" + customVariableUpdatedArgs.Value.ToString());
                currentEpisode.currentProgressValue = int.Parse(customVariableUpdatedArgs.Value);
                Debug.Log("Сохранение переменной из custom variable");
            }
        }
    }




    private void CheckAndChangeStateEpisode()
    {

        foreach (var currentEpisode in Configuration.currentDataEpisodes.currentEpisodes)
        {
            float currentProgress = currentEpisode.currentProgressValue;
            float progressInFloat = currentProgress / currentEpisode.maxProgressValue;
            if ((currentEpisode.currentEpisodeState == StateEpisodeItem.Progress) && (progressInFloat == 1f))
            {
                currentEpisode.currentEpisodeState = StateEpisodeItem.WaitClaiming;
            }
        }
    }


    /// <summary>
    /// Get cost for reset All Season
    /// </summary>
    /// <returns></returns>
    public int GetCostResetSeason()
    {
        var summaryResetCrystal = 0;
        foreach (var currentEpisode in Configuration.currentDataEpisodes.currentEpisodes)
        {
            summaryResetCrystal += currentEpisode.resetCrystal;
        }
        return summaryResetCrystal;
    }
    /// <summary>
    /// Resets Season Data
    /// </summary>
    public void ResetDataSeasonAndSave()
    {
        ResetDataSeason();
        string[] currentSaves = new string[Configuration.currentDataEpisodes.currentEpisodes.Count];
        for (int i = 0; i < currentSaves.Length; i++)
        {
            currentSaves[i] = Configuration.currentDataEpisodes.currentEpisodes[i].saveName;
            stateManager.GameSlotManager.DeleteSaveSlot(currentSaves[i]);
        }

        Configuration.dataCurrencyAndResources.countCrystals -= GetCostResetSeason();

    }
    private void ResetDataSeason()
    {
        for (int i = 0; i < Configuration.currentDataEpisodes.currentEpisodes.Count; i++)
        {
            Configuration.currentDataEpisodes.currentEpisodes[i].epsiodeName = Configuration.defaultDataEpisodes.currentEpisodes[i].epsiodeName;
            Configuration.currentDataEpisodes.currentEpisodes[i].description = Configuration.defaultDataEpisodes.currentEpisodes[i].description;
            Configuration.currentDataEpisodes.currentEpisodes[i].saveName = Configuration.defaultDataEpisodes.currentEpisodes[i].saveName;
            Configuration.currentDataEpisodes.currentEpisodes[i].nameProgressVariable = Configuration.defaultDataEpisodes.currentEpisodes[i].nameProgressVariable;
            Configuration.currentDataEpisodes.currentEpisodes[i].currentProgressValue = Configuration.defaultDataEpisodes.currentEpisodes[i].currentProgressValue;
            Configuration.currentDataEpisodes.currentEpisodes[i].maxProgressValue = Configuration.defaultDataEpisodes.currentEpisodes[i].maxProgressValue;
            Configuration.currentDataEpisodes.currentEpisodes[i].splashImage = Configuration.defaultDataEpisodes.currentEpisodes[i].splashImage;
            Configuration.currentDataEpisodes.currentEpisodes[i].rewardsCrystal = Configuration.defaultDataEpisodes.currentEpisodes[i].rewardsCrystal;
            Configuration.currentDataEpisodes.currentEpisodes[i].unblockable = Configuration.defaultDataEpisodes.currentEpisodes[i].unblockable;
            Configuration.currentDataEpisodes.currentEpisodes[i].currentEpisodeState = Configuration.defaultDataEpisodes.currentEpisodes[i].currentEpisodeState;
            Configuration.currentDataEpisodes.currentEpisodes[i].fullSplashImage = Configuration.defaultDataEpisodes.currentEpisodes[i].fullSplashImage;
            Configuration.currentDataEpisodes.currentEpisodes[i].energyCost = Configuration.defaultDataEpisodes.currentEpisodes[i].energyCost;
            Configuration.currentDataEpisodes.currentEpisodes[i].fullDescription = Configuration.defaultDataEpisodes.currentEpisodes[i].fullDescription;
        }
        SetNaninovelVariable();
        OnEpisodesChanges?.Invoke();
    }
    /// <summary>
    /// Resets Episode Data
    /// </summary>
    /// <param name="i">Index Episode</param>
    public void ResetDataEpisodeAndSave(int i)
    {

        ResetDataEpisode(i);
        stateManager.GameSlotManager.DeleteSaveSlot(Configuration.currentDataEpisodes.currentEpisodes[i].saveName);


    }
    private void ResetDataEpisode(int i)
    {
        Configuration.currentDataEpisodes.currentEpisodes[i].epsiodeName = Configuration.defaultDataEpisodes.currentEpisodes[i].epsiodeName;
        Configuration.currentDataEpisodes.currentEpisodes[i].description = Configuration.defaultDataEpisodes.currentEpisodes[i].description;
        Configuration.currentDataEpisodes.currentEpisodes[i].saveName = Configuration.defaultDataEpisodes.currentEpisodes[i].saveName;
        Configuration.currentDataEpisodes.currentEpisodes[i].nameProgressVariable = Configuration.defaultDataEpisodes.currentEpisodes[i].nameProgressVariable;
        Configuration.currentDataEpisodes.currentEpisodes[i].currentProgressValue = Configuration.defaultDataEpisodes.currentEpisodes[i].currentProgressValue;
        Configuration.currentDataEpisodes.currentEpisodes[i].maxProgressValue = Configuration.defaultDataEpisodes.currentEpisodes[i].maxProgressValue;
        Configuration.currentDataEpisodes.currentEpisodes[i].splashImage = Configuration.defaultDataEpisodes.currentEpisodes[i].splashImage;
        Configuration.currentDataEpisodes.currentEpisodes[i].rewardsCrystal = Configuration.defaultDataEpisodes.currentEpisodes[i].rewardsCrystal;
        Configuration.currentDataEpisodes.currentEpisodes[i].unblockable = Configuration.defaultDataEpisodes.currentEpisodes[i].unblockable;
        Configuration.currentDataEpisodes.currentEpisodes[i].currentEpisodeState = Configuration.defaultDataEpisodes.currentEpisodes[i].currentEpisodeState;
        Configuration.currentDataEpisodes.currentEpisodes[i].fullSplashImage = Configuration.defaultDataEpisodes.currentEpisodes[i].fullSplashImage;
        Configuration.currentDataEpisodes.currentEpisodes[i].energyCost = Configuration.defaultDataEpisodes.currentEpisodes[i].energyCost;
        Configuration.currentDataEpisodes.currentEpisodes[i].fullDescription = Configuration.defaultDataEpisodes.currentEpisodes[i].fullDescription;
        SetNaninovelVariable();
        OnEpisodesChanges?.Invoke();
    }

    public void ClaimAllEpisodes()
    {
        for (int i = 0; i < Configuration.currentDataEpisodes.currentEpisodes.Count; i++)
        {
            if (Configuration.currentDataEpisodes.currentEpisodes[i].currentEpisodeState == StateEpisodeItem.WaitClaiming)
            {
                Configuration.dataCurrencyAndResources.countCrystals += Configuration.currentDataEpisodes.currentEpisodes[i].rewardsCrystal;
                Configuration.currentDataEpisodes.currentEpisodes[i].currentEpisodeState = StateEpisodeItem.Claimed;

                if (Configuration.currentDataEpisodes.currentEpisodes.Count >= i + 2)
                {
                    Configuration.currentDataEpisodes.currentEpisodes[i + 1].currentEpisodeState = StateEpisodeItem.Progress;
                }
            }
        }
        OnEpisodesChanges?.Invoke();
    }

    public bool CheckStatesThatWaitClimbing()
    {
        for (int i = 0; i < Configuration.currentDataEpisodes.currentEpisodes.Count; i++)
        {
            if (Configuration.currentDataEpisodes.currentEpisodes[i].currentEpisodeState == StateEpisodeItem.WaitClaiming)
            {
                return true;
            }
        }
        return false;
    }

    public bool CanAffordRestartSeason()
    {
        var summaryCost = 0;
        for (int i = 0; i < Configuration.currentDataEpisodes.currentEpisodes.Count; i++)
        {
            summaryCost += Configuration.currentDataEpisodes.currentEpisodes[i].resetCrystal;
        }
        if (Configuration.dataCurrencyAndResources.countCrystals < summaryCost)
        {
            return false;
        }
        else if (Configuration.dataCurrencyAndResources.countCrystals >= summaryCost)
        {
            return true;
        }
        return false;
    }


    public List<EpisodeInfo> GetEpisodes()
    {
        return Configuration.currentDataEpisodes.currentEpisodes;
    }
    public EpisodeInfo GetEpisode(int i)
    {
        return Configuration.currentDataEpisodes.currentEpisodes[i];
    }

    public async void LoadingOrCreateEpisodeSave(EpisodeInfo episodeInfo)
    {
        if (stateManager.GameSlotManager.SaveSlotExists(episodeInfo.saveName))
        {
            if (!string.IsNullOrEmpty(episodeInfo.nameScript) &&
                await scriptManager.LoadScriptAsync(episodeInfo.nameScript) is Script titleScript &&
                titleScript.LabelExists("OnLoad"))
            {
                scriptPlayer.ResetService();
                await scriptPlayer.PreloadAndPlayAsync(titleScript, label: "OnLoad");
                await UniTask.WaitWhile(() => scriptPlayer.Playing);
            }
            await stateManager.LoadGameAsync(episodeInfo.saveName);
        }
        else
        {
            if (!string.IsNullOrEmpty(episodeInfo.nameScript) &&
                await scriptManager.LoadScriptAsync(episodeInfo.nameScript) is Script titleScript &&
                titleScript.LabelExists("OnLoad"))
            {
                scriptPlayer.ResetService();
                await scriptPlayer.PreloadAndPlayAsync(titleScript, label: "OnLoad");
                await UniTask.WaitWhile(() => scriptPlayer.Playing);
            }
            string[] currentSaves = new string[1];
            currentSaves[0] = episodeInfo.saveName;
            stateManager.ResetStateAsync(currentSaves,
                () => scriptPlayer.PreloadAndPlayAsync(episodeInfo.nameScript)).Forget();
        }
    }

}
