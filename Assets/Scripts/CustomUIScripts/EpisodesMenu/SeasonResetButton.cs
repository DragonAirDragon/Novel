namespace Naninovel.UI
{
    public class SeasonResetButton : ScriptableButton
    {
        private IUIManager uiManager;

        protected override void Awake()
        {
            base.Awake();
            uiManager = Engine.GetService<IUIManager>();
        }

        protected override void OnButtonClick()
        {
            var seasonReloadUI = uiManager.GetUI<ISeasonReloadUI>();
            if (seasonReloadUI is null) return;
            seasonReloadUI.Show();
        }
    }
}
