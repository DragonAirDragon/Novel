using UnityEngine;
using UnityEngine.UI;

namespace Naninovel.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class SeasonReloadMenu : CustomUI, ISeasonReloadUI
    {
        private IUIManager uiManager;
        public Button buttonRestart;
        private EpisodeService episodeService;
        public Text textCostSeasonReset;
        protected override void Awake()
        {
            base.Awake();
            uiManager = Engine.GetService<IUIManager>();

        }
        protected override void Start()
        {
            base.Start();

            episodeService = Engine.GetService<EpisodeService>();
            var episodesUI = uiManager.GetUI<IEpisodesUI>();
            buttonRestart.onClick.AddListener(() =>
            {
                episodeService.ResetDataSeasonAndSave();
                Hide();
                if (episodesUI is null) return;
                episodesUI.Show();
            });
        }
        public void UpdateCostText()
        {
            textCostSeasonReset.text = episodeService.GetCostResetSeason().ToString();
        }
    }
}