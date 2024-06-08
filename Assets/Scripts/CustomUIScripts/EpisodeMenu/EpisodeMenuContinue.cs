using UnityEngine;
namespace Naninovel.UI
{
    public class EpisodeMenuContinue : ScriptableButton
    {
        private EpisodeMenu episodeMenu;
        private EpisodesMenu episodesMenu;
        private TitleMenu titleMenu;
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
            episodesMenu = uiManager.GetUI<EpisodesMenu>();
            titleMenu = uiManager.GetUI<TitleMenu>();
            episodeMenu.LoadingEpisodeSave();
            episodeMenu.Hide();

        }
    }
}