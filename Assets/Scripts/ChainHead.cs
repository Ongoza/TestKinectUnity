using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainHead : MonoBehaviour
{
    // public string name;
    public Transform previusHead;
    public Vector3 startPosition;
    public float smoothing = 1f;  
    public float rotationSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log(this.name);
        if(previusHead != null){
            startPosition = this.transform.position - previusHead.transform.position;
            // Debug.Log(this.name+": "+this.previusHead.position.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 deltaPosition = previusHead.transform.position + startPosition;
        float delta = deltaPosition.magnitude;
        if(delta < 7)
        {
            // Debug.Log(this.name + ": " + delta.ToString());
            // if(previusHead.transform.position - ){
            this.transform.position = Vector3.Lerp(this.transform.position, deltaPosition, smoothing * Time.deltaTime);
            
            // Quaternion targetRotation = Quaternion.LookRotation(deltaPosition.normalized);
            // this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            // Debug.Log(this.name+": "+delta.ToString());
        }
    }
}
