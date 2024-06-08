using Naninovel;
using Naninovel.UI;
using UnityEngine;

public class EpisodeReloadMenuAceptButton : ScriptableButton
{
    private EpisodeReloadMenu episodeReloadMenu;

    protected override void Awake()
    {
        base.Awake();
        episodeReloadMenu = GetComponentInParent<EpisodeReloadMenu>();

    }
    protected override void OnButtonClick() => ResetEpisode();
    private void ResetEpisode()
    {
        episodeReloadMenu.Hide();
        Engine.GetService<EpisodeService>().ResetDataEpisodeAndSave(episodeReloadMenu.index);
    }
}