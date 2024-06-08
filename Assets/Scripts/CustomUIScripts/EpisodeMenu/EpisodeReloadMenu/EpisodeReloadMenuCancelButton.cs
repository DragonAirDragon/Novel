using Naninovel;
using Naninovel.UI;
using UnityEngine;

public class EpisodeReloadMenuCancelButton : ScriptableButton
{
    private EpisodeReloadMenu episodeReloadMenu;
    private IUIManager uiManager;

    protected override void Awake()
    {
        base.Awake();
        episodeReloadMenu = GetComponentInParent<EpisodeReloadMenu>();
        uiManager = Engine.GetService<IUIManager>();

    }
    protected override void OnButtonClick() => HideMenu();
    private void HideMenu()
    {
        episodeReloadMenu.Hide();
        var episodeMenu = uiManager.GetUI<EpisodeMenu>();
        if (episodeMenu is null) return;
        episodeMenu.Show();
    }

}