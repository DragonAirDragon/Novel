using System;
using System.Collections;
using System.Collections.Generic;
using Naninovel;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class LinkHandlerForTMPText : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TMP_Text _tmpTextBox;
    [SerializeField] private TMP_Text _translateTextBox;
    [SerializeField] private Canvas _canvasToCheck;
    [SerializeField] private Toggle button;
    private Camera cameraToUse;

    public string enterDialogue;
    public string EnterDialogue
    {
        get { return enterDialogue; }
        set
        {
            enterDialogue = value;
            SetText();
        }
    }


    public delegate void ClickOnLinkEvent(string keyword, Vector3 touchPosition);
    public static event ClickOnLinkEvent OnClickedOnLinkEvent;

    private void Awake()
    {
        _canvasToCheck = GetComponentInParent<Canvas>();
        if (_canvasToCheck.renderMode == RenderMode.ScreenSpaceOverlay)
            cameraToUse = null;
        else
            cameraToUse = _canvasToCheck.worldCamera;

        OnClickedOnLinkEvent += ShowPopUpWindow;
    }

    public void ShowPopUpWindow(string enter, Vector3 touchPosition)
    {
        var uiManager = Engine.GetService<IUIManager>();
        var translatePopUpWindow = uiManager.GetUI<TranslatePopUpWindow>();
        translatePopUpWindow?.Show();
        translatePopUpWindow?.Set(FormatWord(enter), touchPosition);

    }
    public void SetText()
    {
        var translateService = Engine.GetService<TranslateService>();
        string translatedText = translateService.GetTranslatedText(EnterDialogue);


        button.onValueChanged.RemoveAllListeners();
        button.onValueChanged.AddListener((value) =>
        {
            if (value)
            {
                _translateTextBox.text = translatedText;
            }
            else
            {
                _translateTextBox.text = "";
            }
        });




        string[] words = EnterDialogue.Split(' ');
        for (int i = 0; i < words.Length; i++)
        {
            words[i] = $@"<link=""{words[i]}""><style=""Clickable"">{words[i]}</style></link>";
        }
        string s = string.Join(" ", words);
        _tmpTextBox.text = s;
    }

    public string FormatWord(string input)
    {
        string lowerCaseInput = input.ToLower();
        string result = "";
        char[] punctuationMarks = { '.', ',', '!', '?' };  // Явно заданные знаки препинания

        foreach (char c in lowerCaseInput)
        {
            if (Array.IndexOf(punctuationMarks, c) == -1)  // Добавляем символ, если он не в списке знаков препинания
            {
                result += c;
            }
        }
        return result;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector3 mousePosition = new Vector3(eventData.position.x, eventData.position.y, 0);

        var linkTaggedText = TMP_TextUtilities.FindIntersectingLink(_tmpTextBox, mousePosition, cameraToUse);

        if (linkTaggedText != -1)
        {
            TMP_LinkInfo linkInfo = _tmpTextBox.textInfo.linkInfo[linkTaggedText];

            OnClickedOnLinkEvent?.Invoke(linkInfo.GetLinkText(), mousePosition);
        }
    }
}
