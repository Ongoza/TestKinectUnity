using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesCloud : MonoBehaviour
{
    //public string prefab_name;
    public int numberOfObjectsInCircle = 1; // 1 simplest circle from 7 circles other multiply by 4
    public int numberOfCircles = 2; // 
    // public int numberOfObjectsInCircle = 6;
    public float radiusStep = 5f;
    public GameObject myPrefab;
    int pCounter = 1;
    // Start is called before the first frame update
    void Start()
    {
        //GameObject myPrefab = Instantiate(Resources.Load(prefab_name, typeof(GameObject))) as GameObject;
        createNode("_center", new Vector3(0, 0, 0), 0);
        for (int k = 1; k <= numberOfCircles; k++) {
            float radius = radiusStep * k;
            if (numberOfObjectsInCircle >= 1) { 
                int half = (int)(numberOfObjectsInCircle * 2);
                createNode("_0", new Vector3(radius, 0, 0), 0);
                createNode("_"+ half.ToString(), new Vector3(-radius, 0, 0), Mathf.PI);
                float aStep = Mathf.PI / half;
                int objNum = 2;
                for (int i = 1; i < half; i++)
                {
                    float angle = aStep * i;
                    float x = Mathf.Cos(angle) * radius;
                    objNum = (int)((angle <= Mathf.PI/2) ? objNum * 2 : objNum / 2);
                    float smallRadius = Mathf.Sin(angle) * radius;
                    for (int j = 0; j < objNum; j++)
                    {
                        float jAngle = 2 * j * Mathf.PI / objNum ;
                        float y = Mathf.Sin(jAngle) * smallRadius;
                        float z = Mathf.Cos(jAngle) * smallRadius; 
                        createNode(i.ToString() + "_" + j.ToString(), new Vector3(x, y, z), angle);
                    }
                }
            }
        }
        Debug.Log("Total number of objects = " + pCounter.ToString());
    }

    void createNode(string name, Vector3 v, float angle)
    {
        Vector3 pos = transform.position + v;
        float angleDegrees = -angle * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0, angleDegrees, 0);
        GameObject star = Instantiate(myPrefab, pos, rot, transform);
        star.name = "Star_" + name;
        pCounter += 1;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
