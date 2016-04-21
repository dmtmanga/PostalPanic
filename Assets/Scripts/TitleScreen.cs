using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour {

    public GameObject title;
    public GameObject stamp;
    public GameObject startButton;
    public GameObject devScreen;

    private GameObject player;

    public float startWait;
    public float stampWait;

    private bool readyToAnimate = false;
    private bool gameIsReady = false;
    private bool startButtonHit = false;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {
        devScreen.SetActive(true);
    }

    void Update()
    {
        if (gameIsReady)
        {
            if (!startButtonHit)
            {
                if (Input.anyKeyDown)
                {
                    startButtonHit = true;
                    StartCoroutine(StartGame());
                }
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
        // play the start jingle and stop the button animation
        GetComponent<AudioSource>().Stop();
        startButton.GetComponent<StartButtonAudio>().PlayStartJingle();
        yield return new WaitForSeconds(startWait);

        // move the mailman into screen
        PlayerController playerCont = player.GetComponent<PlayerController>();
        AudioSource playerAudio = player.GetComponent<AudioSource>();
        player.transform.position = new Vector3(playerCont.pos[3], playerCont.y_pos, 0f);
        playerAudio.PlayOneShot(playerCont.move);
        yield return new WaitForSeconds(startWait);

        // disable all the title/text elements
        title.SetActive(false);
        stamp.SetActive(false);
        startButton.SetActive(false);

        // move the mailman in to position
        player.transform.position = new Vector3(playerCont.pos[2], playerCont.y_pos, 0f);
        playerAudio.PlayOneShot(playerCont.move);
        yield return new WaitForSeconds(startWait);

        SceneManager.LoadScene("Game01");
    }


    public void ReadyToAnimate()
    {
        readyToAnimate = true;
    }

}


