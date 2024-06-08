
using System.IO;
using Naninovel;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System.Text;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Sirenix.Utilities;
using System.Linq;
using JetBrains.Annotations;

[InitializeAtRuntime]
public class TranslateService : IEngineService<TranslateConfiguration>
{
    public TranslateConfiguration Configuration { get; }

    private IAudioManager audioManager;

    private IScriptPlayer scriptPlayer;

    public event Action onListWordChange;


    public TranslateService(TranslateConfiguration config, IAudioManager audioPlayer, IScriptPlayer scriptPlayer)
    {
        Configuration = config;
        this.audioManager = audioPlayer;
        this.scriptPlayer = scriptPlayer;
    }

    public async UniTask InitializeService()
    {
        foreach (var word in Configuration.dataWords.words)
        {
            word.audioClip = await LoadAudio(word.pathAudio);
        }
    }

    public async UniTask<Word> SearchOrCreateWordAndReturnIt(string enterEnglishWord)
    {

        if (Configuration.dataWords.words.Find(word => word.englishWord == enterEnglishWord) != null)
        {
            return Configuration.dataWords.words.Find(word => word.englishWord == enterEnglishWord);
        }
        else
        {
            return await GetAllForApiAsync(enterEnglishWord);
        }
    }


    public bool CheckExistWord(string enterEnglishWord)
    {
        var foundWord = Configuration.dataWords.words.Find(word => word.englishWord == enterEnglishWord);
        if (foundWord != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddNewWordToList(Word enterWord)
    {
        Configuration.dataWords.AddWord(enterWord);
        Debug.Log($"New word added: {enterWord.englishWord}");
        if (enterWord.audioClip != null)
        {
            SaveAndSerializeAudioClip(enterWord.audioClip, enterWord.englishWord);
            Configuration.dataWords.words.Last().pathAudio = Path.Combine(Application.persistentDataPath, enterWord.englishWord + ".wav");
        }
        onListWordChange?.Invoke();
    }

    public void RemoveWordToList(Word enterEnglishWord)
    {
        Configuration.dataWords.RemoveWord(enterEnglishWord);
        DeleteFile(enterEnglishWord.englishWord + ".wav");
        onListWordChange?.Invoke();
    }


    private void DeleteFile(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log($"Файл {fileName} успешно удалён из persistance директории.");
        }
        else
        {
            Debug.Log($"Файл {fileName} не найден в persistance директории.");
        }
    }

    public async UniTask<AudioClip> LoadAudio(string path)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + path, AudioType.WAV))
        {
            await www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
                return null;
            }
            else
            {
                return DownloadHandlerAudioClip.GetContent(www);
            }
        }
    }

    public void SaveAndSerializeAudioClip(AudioClip clip, string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName + ".wav");

        // Преобразование AudioClip в массив байтов и сохранение его в файл
        SaveWav.Save(filePath, clip);
        Debug.Log($"Audio clip saved at: {filePath}");
    }

    public async UniTask<Word> GetAllForApiAsync(string word)
    {
        var wordClass = new Word();
        wordClass.englishWord = word;
        wordClass.russianWord = await TranslateWordUsingYandexDictionary(word, "en-ru");
        List<string> list = new List<string>();
        await GetWordAudioAndPastOfSpeechUsingMerriamWebster(word, list);
        wordClass.partOfSpeech = list[1];
        wordClass.audioClip = await DownloadAndReturnAudio(list[0]);
        return wordClass;
    }



    private async UniTask<AudioClip> DownloadAndReturnAudio(string audioUrl)
    {
        Debug.Log("Attempting to download audio from: " + audioUrl);  // Логирование URL
        using (UnityWebRequest audioRequest = UnityWebRequestMultimedia.GetAudioClip(audioUrl, AudioType.MPEG))
        {
            await audioRequest.SendWebRequest();

            if (audioRequest.result == UnityWebRequest.Result.ConnectionError || audioRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error downloading audio: " + audioRequest.error + " | URL: " + audioUrl);
            }
            else if (audioRequest.result == UnityWebRequest.Result.Success)
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(audioRequest);
                if (clip != null)
                {
                    Debug.Log("Audio downloaded successfully.");
                    return clip;
                }
                else
                {
                    Debug.LogError("Failed to load clip content.");
                }
            }
            return null;
        }
    }
    private async UniTask<string> TranslateWordUsingMicrosoft(string word, string toLanguage = "ru")
    {
        string url = "https://microsoft-translator-text.p.rapidapi.com/translate?to%5B0%5D=" + toLanguage + "&api-version=3.0&profanityAction=NoAction&textType=plain";
        string requestBody = "[{\"Text\":\"" + word + "\"}]";

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(requestBody);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("X-RapidAPI-Key", "1fea5f80e7msh226781e04475ba0p113967jsn5096b1eceed1");
            request.SetRequestHeader("X-RapidAPI-Host", "microsoft-translator-text.p.rapidapi.com");

            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var json = JSON.Parse(request.downloadHandler.text);
                if (json != null && json[0]["translations"].Count > 0)
                {
                    return json[0]["translations"][0]["text"];
                }
            }
            else
            {
                Debug.LogError("Error in Microsoft Translator API request: " + request.error);
            }

            return null;
        }
    }
    // Free Yandex dictionary (audio not exist)
    private async UniTask<string> TranslateWordUsingYandexDictionary(string word, string langPair)
    {
        string apiKey = Configuration.yandexDictionaryApi;
        string url = $"https://dictionary.yandex.net/api/v1/dicservice.json/lookup?key={apiKey}&lang={langPair}&text={word}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var json = JSON.Parse(request.downloadHandler.text);
                if (json != null && json["def"].Count > 0)
                {
                    foreach (JSONNode def in json["def"].AsArray)
                    {

                        foreach (JSONNode tr in def["tr"].AsArray)
                        {
                            string translatedText = tr["text"];
                            return translatedText;
                        }
                    }
                }
            }
            return null;
        }
    }

    private async UniTask GetWordAudioAndPastOfSpeechUsingMerriamWebster(string word, List<string> list)
    {
        string apiKey = Configuration.merriamWebsterDictionaryApi;
        string url = $"https://dictionaryapi.com/api/v3/references/learners/json/{word}?key={apiKey}";


        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string response = request.downloadHandler.text;
                ParseMerriamWebsterResponse(response, list);
            }
            else
            {
                Debug.LogError("Error in request: " + request.error);
            }
        }

    }

    private void ParseMerriamWebsterResponse(string jsonResponse, List<string> list)
    {
        var json = JSON.Parse(jsonResponse);

        foreach (JSONNode item in json.AsArray)
        {
            string partOfSpeech = item["fl"]; // Получаем часть речи
            JSONNode audioNode = item["hwi"]["prs"][0]["sound"]["audio"]; // Получаем аудио файл

            if (audioNode != null) // Проверяем, что узел аудио существует
            {
                string audio = audioNode.Value; // Получаем значение аудио
                char firstLetter = audio[0];

                list.Add($"https://media.merriam-webster.com/audio/prons/en/us/mp3/{firstLetter}/{audio}.mp3");
                list.Add(partOfSpeech);
                break;
            }
        }
    }

    public void PlayAudioClipDirectly(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogError("Audio clip is null.");
            return;
        }

        // Вы можете добавить параметры, как громкость и группу, если это необходимо
        audioManager.PlayTranslate(clip);
    }




    public string GetTranslatedText(string text)
    {
        if (Configuration.scriptTranslationData.scriptWithTranslation.TryGetValue(text, out var translatedText))
        {
            return translatedText;
        }
        else
        {
            return "Error...";
        }
    }

    public List<Word> GetSavedWord()
    {
        return Configuration.dataWords.words;
    }

    public void DestroyService()
    {

    }

    public void ResetService()
    {

    }

}
