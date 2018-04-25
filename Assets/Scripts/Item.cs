using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType {
    fire,
    water,
    ground,
    lighten,
    HP,
}
public class Item : MonoBehaviour {

    public ItemType curItem;
	// Use this for initialization
	void Start () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player")&& collision.transform.GetComponent<PlayerController>()!=null) {
            collision.transform.GetComponent<PlayerController>().GetElement(curItem);
            Destroy(gameObject);
            
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
