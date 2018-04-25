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
            Vector3 tCameraPos=Vector3.one;
            if (focus.transform.position.y >= transform.position.y + 8)

            {
                float Y= transform.position.y+6;
                tCameraPos = new Vector3(focus.transform.position.x + 6f, Y , transform.position.z);
            }
            else if (focus.transform.position.y < transform.position.y - 8)
            {
                float Y = transform.position.y -4;
                tCameraPos = new Vector3(focus.transform.position.x + 6f, Y, transform.position.z);

            }
            else  {
                tCameraPos = new Vector3(focus.transform.position.x + 6f, transform.position.y, transform.position.z);

            }
            transform.position = tCameraPos;
        }
    }
}
