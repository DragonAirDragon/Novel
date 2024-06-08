using System.Collections;
using System.Collections.Generic;
using Naninovel;
using UnityEngine;

public class WordListReturnButton : ScriptableButton
{
    private WordListWindow wordListWindow;

    protected override void Awake()
    {
        base.Awake();
        wordListWindow = GetComponentInParent<WordListWindow>();
    }
    protected override void OnButtonClick() => HideMenu();
    private void HideMenu()
    {
        wordListWindow.Hide();
    }
}
