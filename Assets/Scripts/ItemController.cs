using UnityEngine;
using System.Collections;

public class ItemController : MonoBehaviour {
    private const float UNITS_P_PIXEL = 0.1565f;

    public int fallDelay;
    public int pointValue;
    private int _currentCD = 0;

	
	void Update () {
        if ( _currentCD <= 0)
            Fall();
        _currentCD -= 1;
	}


    // The item falls. Kinda what it does, y'know?
    void Fall()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - UNITS_P_PIXEL, transform.position.z);
        _currentCD = fallDelay;
    }
}
