namespace Naninovel.UI
{
    public class EpisodeReturnButton : ScriptableButton
    {
        private EpisodesMenu episodesMenu;

        protected override void Awake()
        {
            base.Awake();
            episodesMenu = GetComponentInParent<EpisodesMenu>();
        }
        protected override void OnButtonClick() => HideMenu();
        private void HideMenu()
        {
            episodesMenu.Hide();
        }
    }
}
