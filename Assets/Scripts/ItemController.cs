using UnityEngine;
using System.Collections;

public class ItemController : MonoBehaviour {

    // Public Variables
    public float fallSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Fall();
	}

    // The item falls. Kinda what it does, y'know?
    void Fall()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - fallSpeed, transform.position.z);
    }
}
