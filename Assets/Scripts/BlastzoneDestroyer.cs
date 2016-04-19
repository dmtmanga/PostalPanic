using UnityEngine;
using System.Collections;

public class BlastzoneDestroyer : MonoBehaviour {

    private GameController gameController;
    private GameObject player;
    private Animator playerAnim;

    void Awake()
    {
        GameObject GC = GameObject.FindGameObjectWithTag("GameController");
        gameController = GC.GetComponent<GameController>();

        player = GameObject.FindGameObjectWithTag("Player");
        playerAnim = player.GetComponent<Animator>();

    }

    // When an item collides with the blastzone
    void OnTriggerEnter2D(Collider2D col)
    {
        if (!gameController.isGameOver())
        {
            if (col.tag != "Bomb")
            {
                float playerDistance = Mathf.Abs(col.transform.position.x - player.transform.position.x);
                if (playerDistance < 0.5f)
                    playerAnim.SetBool("NearMiss", true);
                gameController.TakeDamage(1);
            }
        }

        Destroy(col.gameObject);
    }
}
