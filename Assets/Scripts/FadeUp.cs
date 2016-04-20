using UnityEngine;
using System.Collections;

public class FadeUp : MonoBehaviour {

    private const float UNITS_P_PIXEL = 0.1565f;

    private TitleScreen titleScreen;

    public float screenTime;
    public int moveDelay;
    private int _currentCD;
    private bool moving = false;

    void Awake()
    {
        GameObject TS = GameObject.FindGameObjectWithTag("GameController");
        titleScreen = TS.GetComponent<TitleScreen>();
    }

    void Start()
    {
        _currentCD = 0;
        StartCoroutine(StayOnScreen());
    }

    void Update()
    {
        if (moving)
        {
            if (_currentCD <= 0)
                Rise();
            _currentCD--;
        }

        if (transform.position.y > 15)
        {
            titleScreen.ReadyToAnimate();
            Destroy(gameObject);
        }
    }


    IEnumerator StayOnScreen()
    {
        yield return new WaitForSeconds(screenTime);
        moving = true;
    }

    void Rise()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + UNITS_P_PIXEL, transform.position.z);
        _currentCD = moveDelay;
    }

}
