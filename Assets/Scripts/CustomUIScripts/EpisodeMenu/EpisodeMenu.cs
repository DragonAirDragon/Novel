
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Naninovel.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class EpisodeMenu : CustomUI, IEpisodeUI
    {
        private IUIManager uiManager;

        private EpisodesMenu episodesMenu;
        private TitleMenu titleMenu;
        public int index;
        EpisodeService episodeService;
        public Button buttonOpenRestartEpisode;
        public EpisodeInfo episodeInfo;
        public Text textHeaderEpisode;
        public Text textFullDescription;
        public Text rewardsCrystal;
        public Text energyCost;

        public Image fullSplashImage;
        public Slider sliderProgress;
        public Text progressValue;


        public void SetEpisodeInfoAndUpdateUI(int i)
        {
            episodeService = Engine.GetService<EpisodeService>();
            index = i;
            episodeInfo = episodeService.GetEpisode(index);
            textHeaderEpisode.text = episodeInfo.epsiodeName;
            textFullDescription.text = episodeInfo.fullDescription;
            rewardsCrystal.text = episodeInfo.rewardsCrystal.ToString();
            energyCost.text = episodeInfo.energyCost.ToString();
            fullSplashImage.sprite = episodeInfo.fullSplashImage;
            float currentProgress = episodeInfo.currentProgressValue;
            sliderProgress.value = currentProgress / episodeInfo.maxProgressValue;
            progressValue.text = ((int)(currentProgress / episodeInfo.maxProgressValue)).ToString() + "%";
        }

        /// <summary>
        /// Loading Episode
        /// </summary>
        /// <param name="i"></param>
        public void LoadingEpisodeSave()
        {
            uiManager = Engine.GetService<IUIManager>();
            episodesMenu = uiManager.GetUI<EpisodesMenu>();
            titleMenu = uiManager.GetUI<TitleMenu>();
            Hide();
            episodesMenu.Hide();
            titleMenu.Hide();
            episodeService.LoadingOrCreateEpisodeSave(episodeInfo);
        }


    }
}

