using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float xRotateStep = 1f;
    public float yRotateStep = 0f;
    public float zRotateStep = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(xRotateStep, yRotateStep, zRotateStep);
    }
}
