using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class FPS : MonoBehaviour
{
    public float timer, refresh, avgFramerate;
    string display = "{0} FPS";

    // private Text m_Text;
    // Start is called before the first frame update
    void Start()
    {
        // add FPS Timer
/*        GameObject cam = GameObject.Find("Main Camera");
        cam.AddComponent<FPS>();*/
        ///
/*        GameObject cam = GameObject.Find("Main Camera");
        Debug.Log(cam);
        GameObject objCanvas = new GameObject("Canvas");
        Canvas c = objCanvas.AddComponent<Canvas>();
        objCanvas.AddComponent<CanvasRenderer>();
        c.renderMode = RenderMode.ScreenSpaceOverlay;
        objCanvas.AddComponent<CanvasScaler>();
        objCanvas.transform.SetParent(cam.transform);
        // ScreenSpaceCamera  WorldSpace;
        // c.renderMode = RenderMode.ScreenSpaceOverlay;
        // c.localScale = new Vector3(0.014f, 0.014f, 1f);
        GameObject panel = new GameObject("Panel");
        panel.transform.SetParent(objCanvas.transform);
        Image i = panel.AddComponent<Image>();
        GameObject textObject = new GameObject("Text");
        textObject.transform.SetParent(panel.transform);
        m_Text = textObject.AddComponent<Text>();
        m_Text.text = "start";*/
    }

    // Update is called once per frame
    void Update()
    {
        float timelapse = Time.smoothDeltaTime;
        timer = timer <= 0 ? refresh : timer -= timelapse;
        if (timer <= 0) avgFramerate = (int)(1f / timelapse);
        // m_Text.text = string.Format(display, avgFramerate.ToString());
    }
    void OnGUI()
    {
        GUI.Label(new Rect(200, 20, 280, 40), string.Format(display, avgFramerate.ToString()));
        //GUI.Label(new Rect(40, 40, 140, 80), "Hello World!");
        //Debug.Log("gui!!!");
    }
}