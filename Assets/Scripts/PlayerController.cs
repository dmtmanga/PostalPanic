using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    //Constants
    private const int LANES = 4;
    public float[] pos = new float[LANES] { -3.5f, -1.5f, 1.5f, 3.5f };
    public int lane;
    public float y_pos = -3.5f;
    public int move_CD = 10; //frames
    private int remaining_move_CD = 0;


	// Use this for initialization
	void Start () {
        // make sure starting lane is valid
        if (lane < 0)
            lane = 0;
        else if (lane > LANES-1)
            lane = LANES-1;

        // set starting position
        transform.position = new Vector3(pos[lane], y_pos, 0f);
	}
	
	// Update is called once per frame
	void Update () {
        if (remaining_move_CD == 0){
            MovePlayer();
            remaining_move_CD = move_CD;
        }

        // tick down movement CD
        remaining_move_CD -= 1;
        if (remaining_move_CD < 0)
            remaining_move_CD = 0;
	}

    void MovePlayer ()
    {
        float direction = Input.GetAxis("Horizontal");
        int move = 0;

        // going right or left?
        if (direction < 0)
            move = -1;
        else if (direction > 0)
            move = 1;

        // if player isn't headed out of bounds, then commit the move
        if (lane + move >= 0 && lane + direction < LANES)
            lane = lane + move;

        // update player position
        transform.position = new Vector3(pos[lane], y_pos, 0f);
    }

}
