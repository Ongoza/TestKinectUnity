using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LHandFollow : MonoBehaviour{
    public Transform RHand;
    public float dropStep = 1.5f;
    public float dropStart = 1.5f;
    private MainScript mainScr;
    public int counterLines = 0;
    private float curTall;
    // Start is called before the first frame update
    void Start(){
        GameObject f = GameObject.Find("RHand");
        if(f != null){
            RHand = f.transform;
            curTall = Mathf.Abs(RHand.position.y);
        }else{Debug.Log("Error!! Can not find the right hand!");}
        mainScr = Camera.main.GetComponent<MainScript>();
        if(mainScr != null){
        }else{Debug.Log("Error!! Can not find the main camera script!");}
    }

    // Update is called once per frame
    void Update(){ 
        if(RHand.position.x != -this.transform.position.x){
            this.transform.position = new Vector3(-RHand.position.x, RHand.position.y, RHand.position.z);
            mainScr._drops._root.transform.position = new Vector3(0, RHand.position.y, 0);
            float delta = Mathf.Abs(RHand.position.y);
            if(counterLines == 0){
                if(delta >= curTall + dropStart){
                    Debug.Log("The first line start " + counterLines +" x="+ RHand.position.x);
                    counterLines++;
                    curTall = delta;
                    mainScr._drops.AddLayer(Mathf.Abs(RHand.position.x));                    
                }
            }else{
                if(delta >= curTall + dropStep){
                    Debug.Log("New line start "+ counterLines + " x=" + RHand.position.x);
                    counterLines++;
                    curTall = delta;
                    mainScr._drops.AddLayer(Mathf.Abs(RHand.position.x));
                }
            }
        }
    }
}
