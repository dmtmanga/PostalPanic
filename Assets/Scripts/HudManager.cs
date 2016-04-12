using UnityEngine;
using System.Collections;

public class HudManager : MonoBehaviour {

    // Constants
    private const int MAX_HP = 3;

    // Prefabs
    public GameObject fullHeart;

    // Private Variabes
    private int _HP;
    private GameObject _Heart1;
    private GameObject _Heart2;
    private GameObject _Heart3;
    private GameObject _FHeart1 = null;
    private GameObject _FHeart2 = null;
    private GameObject _FHeart3 = null;

	// Use this for initialization
	void Start () {
        _HP = MAX_HP;
        _Heart1 = GameObject.FindGameObjectWithTag("Heart1");
        _Heart2 = GameObject.FindGameObjectWithTag("Heart2");
        _Heart3 = GameObject.FindGameObjectWithTag("Heart3");

        UpdateHealth();
	}
	
	// Update is called once per frame
	void Update () {

	}

    void GameOver()
    {
        Time.timeScale = 0f;
    }

    public void TakeDamage()
    {
        _HP -= 1;
        UpdateHealth();
    }

    void UpdateHealth()
    {
        if (_HP == 3)
            UpdateHealthFiller(3);
        else if (_HP == 2)
        {
            GameObject.Destroy(_FHeart3);
            UpdateHealthFiller(2);
        }
        else if (_HP == 1)
        {
            GameObject.Destroy(_FHeart3);
            GameObject.Destroy(_FHeart2);
            UpdateHealthFiller(1);
        }
        else // HP 0 - He's dead, Jim
        {
            GameObject.Destroy(_FHeart3);
            GameObject.Destroy(_FHeart2);
            GameObject.Destroy(_FHeart1);
            //GameOver();
        }
    }


    // Recursive function to update heart display based on HP
    void UpdateHealthFiller( int heart)
    {
        if (heart == 1)
        {
            if (_FHeart1 == null)
                _FHeart1 = (GameObject) Instantiate(fullHeart, _Heart1.transform.position, new Quaternion());
        }
        if (heart == 2)
        {

            if (_FHeart2 == null)
                _FHeart2 = (GameObject)Instantiate(fullHeart, _Heart2.transform.position, new Quaternion());
            UpdateHealthFiller(heart - 1);
        }
        if (heart == 3) 
        {
            if (_FHeart3 == null)
                _FHeart3 = (GameObject)Instantiate(fullHeart, _Heart3.transform.position, new Quaternion());
            UpdateHealthFiller(heart - 1);
        }
        
    }
}
