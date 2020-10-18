// shader rundom value
// float2 screenUV = frac(sin(dot(uv,float2(12.9898,78.233)))*43758.5453123);

// В центре капля. 
// Количество в круге зависит от радиуса и шага. 
// Максимальный слой состоит из 5 кругов. 
// Количество элементов в каждом круге зависит от его номера. 
// Ширина нового слоя отражается на земле в виде диска. 
// Диск расположен на расстоянии максимального радиуса.
// Все слои слегка выпуклые по центру. 
// Цвет зависит от количества кругов в слое. Чем кругов больше тем насыщенее.
// Размер объекта Размер шага Максимальный радиус Максимальная высота.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainScript : MonoBehaviour
{
    
    public DropsLayer _drops;
    private Text fpsText;
    public float timer, refresh, avgFramerate;
    public string nameScript = "MainScript";
    public Transform catTransform;
    private Transform[] catDots;
    public SkinnedMeshRenderer cat;
    // private Mesh meshCat;
    private GameObject rootCat;
    public GameObject myPrefab;
    private string prefab_name = "Star_Yellow";
    // Start is called before the first frame update
    void Start()
    {
        _drops = new DropsLayer("CloudDrops");
        rootCat = GameObject.Find("rootCat");
        catTransform = rootCat.transform;
        //Debug.Log(catTransform.GetType());
        // this.myPrefab = Object.Instantiate(Resources.Load(prefab_name, typeof(GameObject))) as GameObject;
         this.myPrefab = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        // Material dropMat = Resources.Load("TrianglePyramidExtrusion", typeof(Material)) as Material;
        Material dropMat = Resources.Load("Drop", typeof(Material)) as Material;
        this.myPrefab.GetComponent<Renderer>().material = dropMat;
        // rootCat = new GameObject("rootCat");
        // GameObject obj = GameObject.Find("FPS");
        // GameObject obj2 = GameObject.Find("TextFPS");
        /*if(obj != null){
            RectTransform rectTransform = obj.GetComponent<RectTransform>();
            RectTransform rectTransform2 = obj2.GetComponent<RectTransform>();
            rectTransform.position = new Vector3(Screen.width - 100, Screen.height - 60, 0);
            rectTransform2.position = new Vector3(Screen.width - 138, Screen.height - 60, 0);
            fpsText = obj.GetComponent<Text>();
            fpsText.text = "";
        }else{fpsText = null;}
*/
        Debug.Log("Screen Height : " + Screen.height +" : "+ Screen.width);

        // GameObject objDrop = new GameObject("RootDrops");
        // _dropsRoot = objDrop.transform;
        // GameObject obj2 = Tools.CreateDrop("d1", _dropsRoot, new Vector3(0, 1, 2));
        // _drops.Add(obj2);

        // if(obj){
        //     // Debug.Log(obj.name);
        //     // LHandFollow lHand = obj.GetComponent<LHandFollow>();
        //     // Debug.Log("Drops: "+lHand.dropStep.ToString());
        //     // Destroy(lHand);
        // }else{
        //     Debug.Log("Can not find");
        // }
        // GameObject cylinder = GameObject.Find("human");
        GameObject cylinder = GameObject.Find("Cat_Lite");
        // GameObject cylinder = GameObject.Find("Ellen_Body");
        // catTransform = cylinder.transform;
        // catTransform = GameObject.Find("human_root").transform;
        //cylinder.BakeMesh(baked);
        if (cylinder != null){
            // Mesh mesh = cylinder.GetComponent<MeshFilter>().mesh;
            cat = cylinder.GetComponent<SkinnedMeshRenderer>();
            Mesh meshCat = new Mesh();
            cat.BakeMesh(meshCat);
            Vector3[] verts = meshCat.vertices;
            catDots = new Transform[verts.Length];
            for (var i = 0; i < verts.Length; i++)
            {
                // Vector3 vec = cylinder.transform.TransformPoint(verts[i]);
                //catDots[i] = Tools3d.CreateDropSmall("sm" + i.ToString(), rootCat.transform, catTransform.TransformPoint(verts[i]), 0.1f, _drops.dropMat);
                catDots[i] = Instantiate(this.myPrefab, this.catTransform.TransformPoint(verts[i]), new Quaternion(0f, 0f, 0f, 0f), this.catTransform).transform;
                catDots[i].localScale = new Vector3(0.1f, 0.1f, 0.1f);
            }
            Debug.Log("Verts Number = " + verts.Length.ToString());
        }
       // _drops = null;
    }

    // Update is called once per frame
    void Update(){
        /*if (fpsText != null){
            float timelapse = Time.smoothDeltaTime;
            timer = timer <= 0 ? refresh : timer -= timelapse;
            if(timer <= 0) avgFramerate = (int) (1f / timelapse);
            fpsText.text = avgFramerate.ToString();
        }*/
        if(_drops != null){
            _drops._root.transform.Rotate(0, 0.2f, 0, Space.Self);
        }
        if( cat != null){
            Mesh mesh = new Mesh();
            cat.BakeMesh(mesh);
            Vector3[] verts = mesh.vertices;
            for (var i = 0; i < verts.Length; i++)
            {
                catDots[i].position = catTransform.TransformPoint(verts[i]);
            }
        }
    }


}
