using System.Collections;
using System.Collections.Generic;
using Naninovel;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WordItem : MonoBehaviour
{
    private Word currentWord;
    public TextMeshProUGUI wordText;
    public Button playAudio;
    public Button delete;
    public Button context;

    private TranslateService translateService;

    private void Awake()
    {
        playAudio.onClick.AddListener(ClickAudioButton);
        delete.onClick.AddListener(ClickDeleteButton);
        context.onClick.AddListener(ClickContextButton);
    }

    private void Start()
    {
        translateService = Engine.GetService<TranslateService>();
    }
    private void ChangeValueFromWord()
    {
        wordText.text = $"{currentWord.englishWord}\n{currentWord.russianWord}";
    }

    public void SetWord(Word enterWord)
    {
        currentWord = enterWord;
        ChangeValueFromWord();
    }

    public void ClickAudioButton()
    {
        translateService.PlayAudioClipDirectly(currentWord.audioClip);
    }
    public void ClickDeleteButton()
    {
        translateService.RemoveWordToList(currentWord);
    }
    public void ClickContextButton()
    {

    }

}
