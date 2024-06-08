using Naninovel;
using Naninovel.UI;
using UnityEngine;


public class EpisodeMenuReloadButton : ScriptableButton
{
    private EpisodeReloadMenu episodeReloadMenu;

    private EpisodeMenu episodeMenu;

    private IUIManager uiManager;

    protected override void Awake()
    {
        base.Awake();
        episodeMenu = GetComponentInParent<EpisodeMenu>();
        uiManager = Engine.GetService<IUIManager>();

    }
    protected override void OnButtonClick() => HideMenu();
    private void HideMenu()
    {
        episodeReloadMenu = uiManager.GetUI<EpisodeReloadMenu>();
        episodeReloadMenu.UpdateUI(episodeMenu.index);
        episodeReloadMenu.Show();
    }
}