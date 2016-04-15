using UnityEngine;
using System.Collections;

public class BlastzoneDestroyer : MonoBehaviour {

    public GameController gameController;


    // When an item collides with the blastzone
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag != "Bomb")
            gameController.TakeDamage(1);
        Destroy(col.gameObject);
    }
}
