using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

public class PulseShader : MonoBehaviour

{
    public float shaderPulseSpeed = 1f;
    private Material m_Material;
    private Renderer rend;
    public int skipCounter = 50;
    private int cur_skipCounter;
    // Start is called before the first frame update
    void Start()
    {
        cur_skipCounter = skipCounter;
        rend = GetComponent<Renderer>();
        //m_Material = GetComponent<Renderer>().sharedMaterial;
        // Debug.Log(m_Material);
        rend.material.SetFloat("_ExtrusionFactor", 0.5f);
        //GetComponent<Renderer>().sharedMaterial
    }

    // Update is called once per frame
    void Update()
    {
        if (cur_skipCounter <= 0) { 
            rend.material.SetFloat("_ExtrusionFactor", Random.Range(0.5f, shaderPulseSpeed));
            cur_skipCounter = skipCounter;
            //Debug.Log("update");
        }
        else
        {
            cur_skipCounter--;
        }

    }
}
