using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
using TMPro;

/// <summary>
/// see here https://lightbuzz.com/speech-recognition-unity/
/// </summary>
public class SpeechRecognitionEngine : MonoBehaviour
{
    public string[] keywords;// = new string[] { "stop", "delete", "by", "yes", "no" };
    public ConfidenceLevel confidence = ConfidenceLevel.Medium;    
    public TextMeshPro results;    
    private int _wordCounter = 0;
    string word = "";
    protected PhraseRecognizer recognizer;        

    private void Start()
    {
        
        if (keywords != null)
        {
            recognizer = new KeywordRecognizer(keywords, confidence);
            recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
            recognizer.Start();
            Debug.Log( recognizer.IsRunning );
        }
        string o = "Mic: ";
        int i = 0;
        foreach (var device in Microphone.devices)
        {
            o += i.ToString() +" "+ device+". ";
        }
        results.text = o;
    }

    private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        string newWord = args.text;
        string s = "";
        if (newWord == word) {
            _wordCounter++;
            s = ": " +_wordCounter.ToString();
        }
        else{
            word = newWord;
            _wordCounter = 0;
        }
        /*switch (word)
        {
            case "stop":
                word += "!!!";
                break;
            case "ready":
                s += "_!";
                break;

        }*/
        results.text = "You said: <b>" + word + "</b>" + s;

    }

    private void Update()
    {
            
        
    }

    private void OnApplicationQuit()
    {
        if (recognizer != null && recognizer.IsRunning)
        {
            recognizer.OnPhraseRecognized -= Recognizer_OnPhraseRecognized;
            recognizer.Stop();
            recognizer.Dispose();
        }
    }
}
