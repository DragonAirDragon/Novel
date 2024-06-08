
using Naninovel;
using Naninovel.UI;

public class TitleEpisodesButton : ScriptableButton
{
    private IUIManager uiManager;

    protected override void Awake()
    {
        base.Awake();

        uiManager = Engine.GetService<IUIManager>();
    }

    protected override void OnButtonClick()
    {
        var episodesUI = uiManager.GetUI<IEpisodesUI>();
        if (episodesUI is null) return;
        episodesUI.Show();
    }
}
