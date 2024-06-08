using System.Collections;
using System.Collections.Generic;
using Naninovel;
using Naninovel.UI;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class EpisodeReloadMenu : CustomUI, IEpisodeReloadUI
{
    public Text nameEpisode;
    public Text costReload;
    public int index;
    public void UpdateUI(int index)
    {
        this.index = index;
        var episodeService = Engine.GetService<EpisodeService>();
        var episodeInfo = episodeService.GetEpisode(this.index);
        nameEpisode.text = "Перезапустить \n " + episodeInfo.epsiodeName + " ?";
        costReload.text = episodeInfo.resetCrystal.ToString();
    }
}
