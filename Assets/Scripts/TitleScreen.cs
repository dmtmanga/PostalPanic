using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour {

    public GameObject title;
    public GameObject stamp;
    public GameObject startButton;
    public GameObject devScreen;

    public float startWait;
    public float stampWait;

    public AudioClip startJingle;

    private bool readyToAnimate = false;
    private bool gameIsReady = false;

    void Start()
    {
        devScreen.SetActive(true);
    }

    void Update()
    {
       if (gameIsReady)
        {
            if (Input.anyKeyDown)
            {
                StartCoroutine(StartGame());
            }
        }
        else if (readyToAnimate)
        {
            StartCoroutine(AnimateScreen());
        }

    }


    IEnumerator AnimateScreen()
    {
        yield return new WaitForSeconds(stampWait);
        stamp.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        startButton.SetActive(true);
        gameIsReady = true;
    }


    IEnumerator StartGame()
    {
        AudioSource startAudio = startButton.GetComponent<AudioSource>();
        startAudio.PlayOneShot(startJingle);
        startButton.GetComponent<Animator>().Stop();
        yield return new WaitForSeconds(startWait);
        SceneManager.LoadScene("Game01");
    }


    public void ReadyToAnimate()
    {
        readyToAnimate = true;
    }

}


