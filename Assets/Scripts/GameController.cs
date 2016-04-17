using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class GameData
{
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
        StartCoroutine (Instructions());
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
        yield return new WaitForSeconds(3f);
        collectMailSpriteObj.GetComponent<SpriteRenderer>().enabled = false;
        dontBlowUpSpriteObj.SetActive(true);
        yield return new WaitForSeconds(4.5f);
        collectMailSpriteObj.SetActive(false);
        dontBlowUpSpriteObj.SetActive(false);
        gameStart = true;
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            for (int i = 0; i < itemsPerWave; i++)
            {
                int spawnIndex = Random.Range(0, 4);
                int rngValue = Random.Range(0, 100);
                int itemIndex; // 0: postcard | 1: letter | 2: package | 3: bomb
                if (rngValue < 20)
                    itemIndex = 0;
                else if (rngValue < 50)
                    itemIndex = 1;
                else if (rngValue < 80)
                    itemIndex = 2;
                else
                    itemIndex = 3;

                spawnPoints[spawnIndex].SpawnItem(itemIndex);
                //Debug.Log("Spawn attempt made at point " + spawnIndex);

                if (gameOver)
                {
                    yield return new WaitForSeconds(restartWait);
                    restartSpriteObj.SetActive(true);
                    restart = true;
                    break;
                }
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);
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
            gameOverSpriteObj.SetActive(true);
            audioSource.Stop();
            audioSource.clip = gameOverBgm;
            audioSource.Play();
            
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
