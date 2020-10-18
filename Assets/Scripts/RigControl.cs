using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using PathCreation;
// using CylinderMeshCreator;
//using System.Diagnostics;
// Under Options / Text-Editor / C# / IntelliSense turn of the lowest tick Box (roughly something like: Do not show elements from Namespaces that are not imported)

#if UNITY_EDITOR
    using UnityEditor;
#endif

public class RigControl : MonoBehaviour
{
    [DllImport("NTKINECTDLL")] private static extern System.IntPtr getKinect();
    [DllImport("NTKINECTDLL")] private static extern System.IntPtr stopKinect(System.IntPtr kinect);
   // [DllImport("NTKINECTDLL")] private static extern unsigned char* getRGBUnity(System.IntPtr kinect);
   // [DllImport("NTKINECTDLL")] private static extern int setSkeleton(System.IntPtr kinect, System.IntPtr data, System.IntPtr state, System.IntPtr id);
    [DllImport("NTKINECTDLL")] private static extern int setSkeleton(System.IntPtr kinect, System.IntPtr data, System.IntPtr state, System.IntPtr id, bool video);
    int bodyCount = 1;
    int jointCount = 25;
    bool showVideo = true;
    public bool isKinect = false;
    private System.IntPtr kinect;
    bool saveToFile = false;
    GameObject[] obj;
    int counter;
    bool isStop = false;
    public GameObject humanoid;
    public bool mirror = true;
    public bool move = true;
    private int curFrame = 0;
    CharacterSkeleton skeleton;
    public bool isShowCubes = false;
    private AnimSkeleton tmp_anim;
    private string result = "";
    public Material material;
    List<Vector3> points = new List<Vector3>();
    List<Vector3> pointsRot = new List<Vector3>();
    void Start()
    {
        Debug.Log("Animation file path: " + Application.persistentDataPath + "/animsave.save");
        tmp_anim = new AnimSkeleton();
        // material = 
        /*        // add FPS Timer
                GameObject cam = GameObject.Find("Main Camera");
                cam.AddComponent<FPS>();
                ///*/
        ///
        skeleton = new CharacterSkeleton(humanoid);
        if (isKinect)
        {
            // get animation from Kinect
            kinect = getKinect();
        }
        else
        {
            // get animation from a file
            //if (File.Exists(Application.persistentDataPath + "/animsave.save"))
            string filePath = Application.streamingAssetsPath + "/animsave.save";
            if (File.Exists(filePath))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(filePath, FileMode.Open);
                tmp_anim = (AnimSkeleton)bf.Deserialize(file);
                file.Close();
            }
            else
            {
                Debug.Log("none resource!!!!");
                Application.Quit();
            }
            //Debug.Log(tmp_anim);
            
            if (isShowCubes)
            {
                obj = new GameObject[bodyCount * jointCount];
                for (int i = 0; i < obj.Length; i++)
                {
                    obj[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    obj[i].transform.position = new Vector3(0, 0, -10);
                    obj[i].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                }
            }

        }
        // Vector3[] points = { new Vector3 { x = 0, y = 0, z = 0 }, new Vector3 { x = 1, y = 1, z = 1}, new Vector3 { x = 0, y = 1, z = 0 } };
        //List<Vector3> points = new List<Vector3> { new Vector3 { x = 0, y = 0, z = 0 }, new Vector3 { x = 1, y = 1, z = 1 }, new Vector3 { x = 0, y = 1, z = 0 } };
        //createPath(points);
    }

    void Update()
    {
        if (isKinect) { 
            float[] data = new float[bodyCount * jointCount * 3];
            int[] state = new int[bodyCount * jointCount];
            int[] id = new int[bodyCount];
            GCHandle gch = GCHandle.Alloc(data, GCHandleType.Pinned);
            GCHandle gch2 = GCHandle.Alloc(state, GCHandleType.Pinned);
            GCHandle gch3 = GCHandle.Alloc(id, GCHandleType.Pinned);
            int n = setSkeleton(kinect, gch.AddrOfPinnedObject(), gch2.AddrOfPinnedObject(), gch3.AddrOfPinnedObject(), showVideo);
            gch.Free();
            gch2.Free();
            gch3.Free();
            // Debug.Log($"skeletons number {n} data {data.Length}  state {state.Length} id {id.Length}");
            if (n > 0)
            {
                if (isShowCubes)
                {
                    updateCubes(data, n);
                }
                skeleton.set(data, state, 0, mirror, move);
                if (saveToFile)
                {
                    AnimSkeletonItem tmp_item = new AnimSkeletonItem();
                    tmp_item.data = data;
                    tmp_item.state = state;
                    tmp_anim.anim.Add(tmp_item);
                }
            }
        }
        else {
            //if (curFrame < tmp_anim.anim.Count) {                
            if (curFrame < 100) {
                Transform t = skeleton.set(tmp_anim.anim[curFrame].data, tmp_anim.anim[curFrame].state, 0, mirror, move);
                points.Add(t.position);
                pointsRot.Add(t.eulerAngles);
                // Debug.Log(curFrame.ToString()+ " " + t.eulerAngles.ToString("F3"));
                curFrame++;
            }
            else
            {
               // curFrame = 0;
                
                if (!isStop) {
                    //List<Vector3> points2 = new List<Vector3> { new Vector3 { x = 0.1f, y = 0.1f, z = 0.1f }, new Vector3 { x = 1f, y = 1f, z = 1f }, new Vector3 { x = 0f, y = 1f, z = 0f } };
                    createPath(points);                    
                    // Application.Quit();
                    isStop = true;
                }
#if UNITY_EDITOR
               // Debug.Log("Stop the app!!");
                //UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
        }
    }

    void updateCubes(float[] data, int n)
    {
        int idx = 0;
        for (int i = 0; i < bodyCount; i++)
        {
            for (int j = 0; j < jointCount; j++)
            {
                if (i < n)
                {
                    float x = data[idx++], y = data[idx++], z = data[idx++];
                    obj[i * jointCount + j].transform.position = new Vector3(x, y, z);
                    obj[i * jointCount + j].GetComponent<Renderer>().material.color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
                }
                else
                {
                    obj[i * jointCount + j].transform.position = new Vector3(0, 0, -10);
                    obj[i * jointCount + j].GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                }
            }
        }
    }

    void OnApplicationQuit()
    {
        if (isShowCubes)
        {
            Debug.Log("Application ending after " + Time.time + " seconds");
            for (int i = 0; i < obj.Length; i++)
            {
                Destroy(obj[i]);
            }
        }
        stopKinect(kinect);
        if (saveToFile)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/animsave.save");
            bf.Serialize(file, tmp_anim);
            file.Close();
        }
        /*foreach (System.Diagnostics.ProcessModule mod in System.Diagnostics.Process.GetCurrentProcess().Modules)
        {
            if (mod.ModuleName == "NTKINECTDLL")
            {
                FreeLibrary(mod.BaseAddress);
            }
        }*/
    }

    GameObject createPath(List<Vector3> points)
    {
        Debug.Log("HandPath");
        GameObject obj = new GameObject("HandPath");
        PathCreator pc = obj.AddComponent<PathCreator>();
        BezierPath bp = new BezierPath(points, false, PathSpace.xyz);
        pc.bezierPath = bp;
        //Tools3d.CreateMesh("TestMesh", pc.path, material:material);
        return obj;

    }
}
