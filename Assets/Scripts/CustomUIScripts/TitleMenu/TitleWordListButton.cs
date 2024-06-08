using System.Collections;
using System.Collections.Generic;
using Naninovel;
using UnityEngine;

public class TitleWordListButton : ScriptableButton
{
    private IUIManager uiManager;

    protected override void Awake()
    {
        base.Awake();
        uiManager = Engine.GetService<IUIManager>();
    }

    protected override void OnButtonClick()
    {
        var episodesUI = uiManager.GetUI<IWordListUI>();
        if (episodesUI is null) return;
        episodesUI.Show();
    }
}
