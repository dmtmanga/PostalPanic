using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    private GameController gameController;
    public GameObject explosion;

    public float[] pos = new float[4];
    public int lane;
    public float y_pos;

    private Animator anim;
    private AudioSource source;
    public AudioClip explode;
    public AudioClip exploderp;
    public AudioClip pickup;

    void Awake()
    {
        GameObject GC = GameObject.FindGameObjectWithTag("GameController");
        gameController = GC.GetComponent<GameController>();
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

	void Start () {
        // ensure valid starting lane
        lane = Mathf.Clamp(lane, 0, 3);

        // set starting position
        transform.position = new Vector3(pos[lane], y_pos, 0f);
	}
	

	void Update () {
        MovePlayer();
	}


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Bomb")
        {
            source.pitch = 1f;
            source.PlayOneShot(explode);
            Instantiate(explosion, transform.position, Quaternion.identity);
            gameController.TakeDamage(3);
            Destroy(col.gameObject);
        }
        else
        {
            source.pitch = 0.75f;
            source.PlayOneShot(pickup);
            anim.SetTrigger("ItemPickUp");
            ItemController item = col.gameObject.GetComponent<ItemController>();
            gameController.Score(item.NumOfPoints());
            Destroy(col.gameObject);
        }
    }


    // Checks for input and moves the player
    void MovePlayer ()
    {
        // check for input
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            lane -= 1;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            lane += 1;
        }

        // update player position
        lane = Mathf.Clamp(lane, 0, 3);
        transform.position = new Vector3(pos[lane], y_pos, 0f);
    }

}
