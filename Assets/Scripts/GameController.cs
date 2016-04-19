using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class GameData
{
    public static bool firstPlay = true;
    public static int score1 = 0;
    public static int score2 = 0;
}

public class GameController : MonoBehaviour {
    public GameObject player;
    private Animator playerAnim;
    private AudioSource audioSource;
    public AudioClip bgm;
    public AudioClip gameOverBgm;
    public AudioClip itemMiss;
    public AudioClip[] waveAudioMixer = new AudioClip[3];
    public int wavesPerAudioClip;

    private bool gameStart;
    private bool gameOver;
    private bool restart;

    public int itemsPerWave;
    public float startWait;
    public float restartWait;
    public float spawnWait;
    public float waveWait;

    private int score;
    public GUIText scoreText;
    public GameObject gameOverSpriteObj;
    public GameObject restartSpriteObj;
    public GameObject collectMailSpriteObj;
    public GameObject dontBlowUpSpriteObj;

    public GameObject fullHeartPrefab;
    private int hp;
    public GameObject[] heartSlots = new GameObject[3];
    private GameObject[] hearts = new GameObject[3];

    public ItemSpawner[] spawnPoints = new ItemSpawner[4];

    void Awake()
    {
        playerAnim = player.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        audioSource.clip = bgm;
        audioSource.Play();
        gameStart = false;
        gameOver = false;
        restart = false;
        score = 0;
        scoreText.text = "" + score;
        gameOverSpriteObj.SetActive(false);
        restartSpriteObj.SetActive(false);
        collectMailSpriteObj.SetActive(false);
        dontBlowUpSpriteObj.SetActive(false);
        hp = 3;
        UpdateHealthUI();
        if (GameData.firstPlay)
            StartCoroutine(Instructions());
        StartCoroutine (SpawnWaves());
    }

    void Update()
    {
        if (restart)
            if (Input.GetKeyDown (KeyCode.Space))
                SceneManager.LoadScene("Game01");

        if (gameStart && !gameOver)
            Score(1);
    }

    IEnumerator Instructions()
    {
        collectMailSpriteObj.SetActive(true);
        yield return new WaitForSeconds(3.2f);
        collectMailSpriteObj.GetComponent<SpriteRenderer>().enabled = false;
        dontBlowUpSpriteObj.SetActive(true);
        yield return new WaitForSeconds(4.3f);
        collectMailSpriteObj.SetActive(false);
        dontBlowUpSpriteObj.SetActive(false);
        gameStart = true;
    }

    IEnumerator SpawnWaves()
    {
        int waveCountDown = wavesPerAudioClip;

        if (GameData.firstPlay)
            yield return new WaitForSeconds(startWait);
        else
            yield return new WaitForSeconds(2f);
        gameStart = true;
        while (true)
        {
            int prevClip = 0;           // to make sure the same audio clip doesn't play twice

            // wave begins
            for (int i = 0; i < itemsPerWave; i++)
            {
                // pick spawn point
                int spawnIndex = Random.Range(0, 4);
                // pick item
                int rngValue = Random.Range(0, 100);
                int itemIndex;          // 0: postcard | 1: letter | 2: package | 3: bomb
                if (rngValue < 20)
                    itemIndex = 0;
                else if (rngValue < 50)
                    itemIndex = 1;
                else if (rngValue < 80)
                    itemIndex = 2;
                else
                    itemIndex = 3;

                // spawn the selected item from the selected spawn point
                spawnPoints[spawnIndex].SpawnItem(itemIndex);

                if (gameOver)
                {
                    // spawn only 1 item and set restart flag to true after a pause
                    yield return new WaitForSeconds(restartWait);
                    restartSpriteObj.SetActive(true);
                    GameData.firstPlay = false;
                    restart = true;
                    break;
                }
                yield return new WaitForSeconds(spawnWait); // single item spawn ends
            }

            if (!gameOver)
            {
                // play voice clip if it should be played
                waveCountDown--;
                if (waveCountDown <= 0)
                {
                    int clip = Random.Range(0, 6);
                    // make sure same clip doesn't play twice
                    while (clip == prevClip)
                    {
                        clip = Random.Range(0, 6);
                    }
                    audioSource.PlayOneShot(waveAudioMixer[clip], 1.5f);
                    waveCountDown = wavesPerAudioClip;
                }
            }
            yield return new WaitForSeconds(waveWait);  // wave ends
        }
    }


    void UpdateHealthUI()
    {
        if (hp == 3)
            UpdateHealthUIFiller(3);
        else if (hp == 2)
        {
            GameObject.Destroy(hearts[2]);
            UpdateHealthUIFiller(2);
        }
        else if (hp == 1)
        {
            for (int i = 1; i < 3; i++)
                GameObject.Destroy(hearts[i]);
            UpdateHealthUIFiller(1);
        }
        else // HP 0 - He's dead, Jim
            for (int i = 0; i < 3; i++ )
                GameObject.Destroy(hearts[i]);
    }


    // Recursive function to update heart display based on HP
    void UpdateHealthUIFiller(int heart)
    {
        if (hearts[heart - 1] == null)
            hearts[heart - 1] = (GameObject) Instantiate(fullHeartPrefab, heartSlots[heart - 1].transform.position, new Quaternion());
        if (heart > 1)
            UpdateHealthUIFiller(heart - 1);
    }


    IEnumerator GameOver()
    {
        if (!gameOver)
        {
            gameOver = true;
            playerAnim.SetBool("Dead", true);
            Destroy(player.GetComponent<PlayerController>());
            GameData.score1 = score;
            yield return new WaitForSeconds(2f);
            audioSource.Stop();
            audioSource.clip = gameOverBgm;
            audioSource.Play();
            gameOverSpriteObj.SetActive(true);
            yield return new WaitForSeconds(1.2f);
            gameOverSpriteObj.GetComponent<SpriteRenderer>().enabled = true;
        }
    }


    public bool isGameOver()
    {
        return gameOver;
    }


    public void TakeDamage( int dmg)
    {
        hp -= dmg;
        hp = Mathf.Clamp(hp, 0, 3);
        
        //Debug.Log("Damage Taken! Current HP is " + _HP);
        if (dmg == 3)
            playerAnim.SetTrigger("BombHit");
        else
        {
            playerAnim.SetTrigger("ItemMiss");
            if(!gameOver)
                audioSource.PlayOneShot(itemMiss);
        }
        if (hp <= 0)
            StartCoroutine(GameOver());
        else
            playerAnim.SetBool("NearMiss", false);
        UpdateHealthUI();
    }

    public void Score( int points)
    {
        score += points;
        scoreText.text = "" + score;
    }
}
