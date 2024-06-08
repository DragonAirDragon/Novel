using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Naninovel;
using Naninovel.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
public class WordListWindow : CustomUI, IWordListUI
{
    private TranslateService translateService;
    private List<WordItem> wordItem = new List<WordItem>();
    public WordItem wordItemPrefab;
    public Transform parentWord;

    protected override void Start()
    {
        translateService = Engine.GetService<TranslateService>();
        translateService.onListWordChange += UpdateListWords;
    }

    public void UpdateListWords()
    {
        foreach (var word in wordItem)
        {
            Destroy(word.gameObject);
        }

        wordItem.Clear();
        foreach (var word in translateService.GetSavedWord())
        {
            wordItem.Add(Instantiate(wordItemPrefab, parentWord));
            wordItem.Last().SetWord(word);
        }
    }
}