using UnityEngine;
using UnityEngine.Windows.Speech;
using System.IO;
using TMPro;

public class SpeechRecognitionEngine 
{
    public string[] keywords = new string[] { "stop", "play", "by", "ok", "next" };
    public ConfidenceLevel confidence = ConfidenceLevel.Medium;
    private char[] characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
    private int savedPoseCounter = 0;
    private string curPoseId = "";
    private CharacterSkeleton curPose;
    private TextMeshPro results;    
    private int _wordCounter = 0;
    string word = "";
    private string poses;
    private int poseVariantCounter = 0;
    private bool isRecording = false;
    protected PhraseRecognizer recognizer;
    //private AnimSkeleton anim;

    public  SpeechRecognitionEngine(CharacterSkeleton animSkeleton, TextMeshPro result)
    {
        this.curPose = animSkeleton;        
        this.isRecording = false;
        this.poses = "";
        this.results = result;
        if (keywords != null)
        {
            this.recognizer = new KeywordRecognizer(keywords, confidence);
            this.recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
            this.recognizer.Start();
            Debug.Log(this.recognizer.IsRunning );
        }
        string o = "Mic: ";
        int i = 0;
        
        foreach (var device in Microphone.devices)
        {
            o += i.ToString() +" "+ device+". ";
        }
        results.text = o;
        Debug.Log(o);
    }


    private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        string newWord = args.text;

        /*if (newWord == this.word) {
            this._wordCounter++;
            s = ": " + this._wordCounter.ToString();
        }
        else{
            this.word = newWord;
            this._wordCounter = 0;
        }*/
        Debug.Log($"You said: {newWord}");
        string s = "";
        switch (newWord)
        {
            case "stop":
                if (this.isRecording) {
                    // BinaryFormatter bf = new BinaryFormatter();
                    //Debug.Log(Quaternion.identity);
                    foreach (Quaternion bone in curPose.bonesRotation) { 
                        poses += bone.x.ToString() +',' + bone.y.ToString() + ',' + bone.z.ToString() + ',' + bone.w.ToString() + ',';
                    }
                    poses += "\n";
                    Debug.Log(poses);
                    s = $"Saved variant N: {poseVariantCounter}";
                    poseVariantCounter++;
                    // string path = Application.persistentDataPath + curPoseId + "_" + savedPoseCounter.ToString() + ".json";
                    // FileStream file = File.Create(Application.streamingAssetsPath + curPoseId +"_"+ savedPoseCounter.ToString() + ".json");
                    // bf.Serialize(file, curPose);
                    // file.Close();


                }
                else
                {
                    Debug.Log("Record did not start!!!");
                    s = "Record did not start!!!";
                }
                break;
            case "by":
                {
                    if (this.isRecording)
                    {

                        //string path = Application.persistentDataPath + curPoseId + "_" + savedPoseCounter.ToString() + ".csv";
                        string path = Application.streamingAssetsPath + "\\" + curPoseId + "_" + savedPoseCounter.ToString() + ".csv";
                        Debug.Log($"path = {path}");
                        using (FileStream fs = new FileStream(path, FileMode.Create))
                        {
                            using (StreamWriter writer = new StreamWriter(fs))
                            {
                                writer.Write(poses);
                            }
                        }
                        poses = "";
                        s = "Pose saved OK!!";
                        this.isRecording = false;
                    }
                    else
                    {
                        Debug.Log("Record did not start!!!");
                        s = "Record did not start!!!";
                    }
                    break;
                }
            case "play":
                if (!this.isRecording) {
                    poses = "";
                    poseVariantCounter = 0;
                    this.savedPoseCounter++;
                    while (this.curPoseId.Length < 8){
                            this.curPoseId += this.characters[Random.Range(0, characters.Length)];
                    }
                    Debug.Log($"Current pose id: {this.curPoseId}");                
                    s = "Start pose recording!!!";
                    this.isRecording = true;
                }
                else
                {
                    Debug.Log("Please stop previus recording!!!");
                    s = "Please stop previus recording!!!";
                }
                break;

        }
        results.text = s;
        Debug.Log($"Word is {s}");

    }


    public void Destroy()
    {
        if (recognizer != null && recognizer.IsRunning)
        {
            recognizer.OnPhraseRecognized -= Recognizer_OnPhraseRecognized;
            recognizer.Stop();
            recognizer.Dispose();
        }
    }
}
