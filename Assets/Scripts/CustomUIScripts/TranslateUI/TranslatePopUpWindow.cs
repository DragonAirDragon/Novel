using System;
using System.Collections;
using System.Collections.Generic;
using Naninovel;
using Naninovel.UI;
using Sirenix.OdinInspector.Editor;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Localization.Editor;
using UnityEngine;
using UnityEngine.UI;

public class TranslatePopUpWindow : CustomUI, ITranslatePopUpUI
{
    public Word word;
    public TranslateService translateService;
    public TMP_Text tmpText;
    public Button audioPlay;
    public Toggle save;
    public RectTransform rectTransform;
    public RectTransform canvasTransform;
    public Canvas canvas;

    private IScriptManager scriptManager;

    private IScriptPlayer scriptPlayer;

    private ILocalizationManager localizationManager;

    protected override void Start()
    {
        base.Start();
        translateService = Engine.GetService<TranslateService>();
        audioPlay.onClick.AddListener(PlaySound);
        save.onValueChanged.AddListener(SaveOrDeleteWord);
    }
    public async void Set(string enterEnglishWord, Vector3 touchPosition)
    {
        SetPosition(touchPosition);
        SetDefault();
        word = await translateService.SearchOrCreateWordAndReturnIt(enterEnglishWord);
        SetParamPopUpWindow(enterEnglishWord);
        TranslateText();
    }

    public void SetPosition(Vector3 touchPosition)
    {
        RectTransform canvasRectTransform = canvasTransform as RectTransform;

        // Преобразуем экранные координаты в координаты канваса
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, touchPosition, canvas.worldCamera, out localPoint))
        {
            // Ограничим позицию внутри канваса, учитывая размеры элемента
            float clampedX = Mathf.Clamp(localPoint.x, -canvasRectTransform.rect.width / 2 + rectTransform.rect.width / 2, canvasRectTransform.rect.width / 2 - rectTransform.rect.width / 2);
            float clampedY = Mathf.Clamp(localPoint.y, -canvasRectTransform.rect.height / 2 + rectTransform.rect.height / 2, canvasRectTransform.rect.height / 2 - rectTransform.rect.height / 2);

            // Присвоим скорректированную позицию к anchoredPosition для позиционирования элемента
            rectTransform.anchoredPosition = new Vector2(clampedX, clampedY);
        }
        else
        {
            Debug.LogError("Failed to convert screen point to local point.");
        }
    }

    private void SetParamPopUpWindow(string enterEnglishWord)
    {
        if ((word.russianWord == null) && (word.partOfSpeech == null))
        {
            save.interactable = false;
            audioPlay.interactable = false;
            save.interactable = false;
            tmpText.text = "нет перевода";
        }
        else
        {
            save.isOn = translateService.CheckExistWord(enterEnglishWord);
            tmpText.text = $"{word.russianWord} ({word.partOfSpeech})";
            audioPlay.interactable = true;
            save.interactable = true;
        }
        if (word.audioClip == null)
        {
            audioPlay.interactable = false;
        }

    }

    private void SetDefault()
    {
        tmpText.text = "Loading...";
        audioPlay.interactable = false;
        save.interactable = false;
    }
    public void PlaySound()
    {
        translateService.PlayAudioClipDirectly(word.audioClip);
    }

    public void SaveOrDeleteWord(bool value)
    {
        if (value)
        {
            translateService.AddNewWordToList(word);
        }
        else
        {
            translateService.RemoveWordToList(word);
        }
    }

    public void TranslateText()
    {
        scriptManager = Engine.GetService<IScriptManager>();

        scriptPlayer = Engine.GetService<IScriptPlayer>();

        localizationManager = Engine.GetService<ILocalizationManager>();




    }
}
