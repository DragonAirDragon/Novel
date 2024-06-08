using UnityEngine;
using Naninovel;

namespace Naninovel.UI
{
    public class EpisodeMenuCancelButton : ScriptableButton
    {
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
            episodeMenu.Hide();
            var episodesUI = uiManager.GetUI<IEpisodesUI>();
            if (episodesUI is null) return;
            episodesUI.Show();
        }
        


    }
}
