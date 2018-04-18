using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarmeraFollow : MonoBehaviour {


    public Transform focus;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (focus != null)
        {

            this.transform.position = new Vector3(focus.transform.position.x+6f,this.transform.position.y,transform.position.z);
        }
    }
}
