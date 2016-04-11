using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    //Constants
    private const int NUM_OF_LANES = 4;
    public float[] pos = new float[NUM_OF_LANES];
    public int lane;
    public float y_pos;
    

	// Use this for initialization
	void Start () {
        // make sure starting lane is valid
        if (lane < 0)
            lane = 0;
        else if (lane > NUM_OF_LANES-1)
            lane = NUM_OF_LANES-1;

        // set starting position
        transform.position = new Vector3(pos[lane], y_pos, 0f);
	}
	
	// Update is called once per frame
	void Update () {
        MovePlayer();
	}

    void MovePlayer ()
    {
        // check for input
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (lane - 1 >= 0)
                lane -= 1;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (lane + 1 < NUM_OF_LANES)
                lane += 1;
        }

        // update player position
        transform.position = new Vector3(pos[lane], y_pos, 0f);
    }

}
