using UnityEngine;
using System.Collections;

public class BlastzoneDestroyer : MonoBehaviour {

    // When an item collides with the blastzone
    void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(col.gameObject);
    }
}
