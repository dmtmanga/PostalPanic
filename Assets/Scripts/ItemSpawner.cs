using UnityEngine;
using System.Collections;

public class ItemSpawner : MonoBehaviour {

    // Item Prefabs
    public GameObject[] items = new GameObject[4];

    // Public Variables
    public int spawnCD; // # of frames @ 60 fps

    // Private Variables
    private int _currentCD;
    

	// Use this for initialization
	void Start () {
	
	}
	

	// Update is called once per frame
	void Update () {
        int index = 0;
        int rngSeed = 0;

        rngSeed = Random.Range(0, 100);

        // WILL BE UPDATED!
        if (rngSeed < 30)
            index = 3;
        else
            index = -1;


	    if (_currentCD <= 0) {
            if (index == -1)
                _currentCD = spawnCD;
            else
            {
                SpawnItem(index);
                _currentCD = spawnCD;
            }
        }

        // update cooldown
        _currentCD -= 1;
	}


    // Spawns a single item
    void SpawnItem( int index )
    {
        Instantiate(items[index], transform.position, new Quaternion());
    }



}
