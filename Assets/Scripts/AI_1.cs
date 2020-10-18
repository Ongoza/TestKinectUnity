using Unity.Barracuda;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Globalization;



public class AI_1 : MonoBehaviour
{
    // Start is called before the first frame update
    public string modelFileName = "Models/mnist";
    public string imageName = "5g";
    public NNModel modelFile; 
    private IWorker _worker;
    private Model _model;
    private const int IMAGE_MEAN = 0;
    private const float IMAGE_STD = 1f;
    private const float MINIMUM_CONFIDENCE = 0.3f;
    public bool Verbose = false;
    public const int IMAGE_SIZE = 28;
    private const string INPUT_NAME = "input";
    private const string OUTPUT_NAME = "output";
    public GameObject plane;
    private Tensor _input;
    public bool ifShowPropability = false;
    // public Texture resTexture;

    //  bool _lock = true;

    private void Start() { 
        var jsonString = Resources.Load<TextAsset>("img"+ imageName);
        //var floatData = JsonUtility.FromJson<ImgData>(jsonString.text);
        //float[] floatData = Array.ConvertAll(jsonString.text.Split(','), float.Parse);
        //List<float> floatData = new List<float>();
        float[] floatData = new float[28*28];
        string[] arrStr = jsonString.text.Split('=');
        int i = 0;        
        foreach (string strf in arrStr) {            
                string[] arrStr2 = strf.Split(',');
                foreach (string strf2 in arrStr2){
                    floatData[i] = float.Parse(strf2, NumberStyles.Float);
                    i++;
                }
            }
        _model = ModelLoader.Load(modelFile, Verbose);
        _worker = _model.CreateWorker();
        // = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, _model);
        //_worker = WorkerFactory.CreateWorker(WorkerFactory.Type.CSharpBurst, _model);

        //StartCoroutine("WaitLoad");
        _input = new Tensor(1, 28, 28, 1, floatData);
        // Tensor input = new Tensor(batch, height, width, channels);
        StartCoroutine(Detect(_input));

    }

    public IEnumerator Detect(Tensor tensor){
        // var inputs = new Dictionary<string, Tensor>();
        // inputs.Add(INPUT_NAME, tensor);
        // yield return StartCoroutine(_worker.ExecuteAsync(inputs));
        // yield return StartCoroutine(_worker.ExecuteAsync(tensor));
        //var output = _worker.PeekOutput(OUTPUT_NAME);
        
        var output = _worker.Execute(tensor).PeekOutput();
        // allow main thread to run until neural network execution has finished
        yield return new WaitForCompletion(output);

        // Debug.Log($"output shape {output.shape}");
        //Debug.Log("classes= " + string.Join(", ", predictedClasses));
        
        
        // Debug.Log(string.Join(", ", output.ArgMax()));
        
        if (ifShowPropability) { 
            float[] predictedClasses = Softmax(output.ToReadOnlyArray());
            var (topResultIndex, topResultScore) = GetTopResult(predictedClasses);
            Debug.Log($"image {imageName} class {topResultIndex} prob= {topResultScore}");
        }
        else
        {
            int cls = output.ArgMax()[0];
            Debug.Log($"image {imageName} class {cls}");
        }
        output.Dispose();

        // var results = ParseOutputs(output);
        //var boxes = FilterBoundingBoxes(results, 5, MINIMUM_CONFIDENCE);

    }

        private Tensor GetImage(string imgPath){
        var graph = Resources.Load(imgPath) as Texture2D;
        var resTexture = new Texture2D(32,32);
        var mat = plane.GetComponent<MeshRenderer>().material;
        //Debug.Log(mat);
        mat.SetTexture("_MainTex", resTexture);
        Color32[] pixels = graph.GetPixels32();
        Color[] pixelsOut = new Color[32 * 32];
        float[] result = new float[28 * 28];
        Debug.Log($"pixels {pixels.Length} result {result.Length}");
        for (int x = 0; x < result.Length; x++)
        {
            int p = ((256 * 256 + pixels[x].r) * 256 + pixels[x].b) * 256 + pixels[x].g;
            int b = p % 256;
            p = Mathf.FloorToInt(p / 256);
            int g = p % 256;
            p = Mathf.FloorToInt(p / 256);
            int r = p % 256;
            float l = (0.2126f * r / 255f) + 0.7152f * (g / 255f) + 0.0722f * (b / 255f);
            result[x] = l;
            int rw = (int)Mathf.Round(l);
            pixelsOut[x] = new Color(rw, rw, rw, 1);
        }
        //for(int i = result.Length; i < pixels.Length; i++){pixelsOut[i] = new Color(1, 1, 1, 1);}
        resTexture.SetPixels(pixelsOut);
        resTexture.Apply();
        return new Tensor(1, 28, 28, 1, result);
    }

    private float[] Softmax(float[] values){
        var maxVal = values.Max(); 
        var exp = values.Select(v => System.Math.Exp(v - maxVal)); 
        var sumExp = exp.Sum(); 
        return exp.Select(v => (float)(v / sumExp)).ToArray();
    }

    private System.ValueTuple<int, float> GetTopResult(float[] predictedClasses)
    {
        return predictedClasses
            .Select((predictedClass, index) => (Index: index, Value: predictedClass))
            .OrderByDescending(result => result.Value)
            .First();
    }

    private IEnumerator WaitLoad(){
        yield return new WaitForSeconds(0.5f);
        var image = GetImage(imageName);
        Debug.Log("test");
        StartCoroutine(Detect(image));    
    }
    public void OnDestroy(){
        _worker.Dispose(); 
        //_model.
        _input.Dispose();
    }

}