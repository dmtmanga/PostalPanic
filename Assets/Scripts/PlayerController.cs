﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public float[] pos = new float[4];
    public int lane;
    public float y_pos;
    private GameObject _gcObject;
    private GameController gameController;


	void Start () {
        _gcObject = GameObject.FindGameObjectWithTag("GameController");
        gameController = _gcObject.GetComponent<GameController>();

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
            gameController.TakeDamage();
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