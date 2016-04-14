using UnityEngine;
using System.Collections;

public class ItemSpawner : MonoBehaviour {
    public GameObject[] items = new GameObject[4];
	

    public void SpawnItem( int index )
    {
        if (index < 0)
            return;
        Instantiate(items[index], transform.position, Quaternion.identity);
        //Debug.Log("SpawnItem func successfully called to spawn item index " + index);
    }



}
