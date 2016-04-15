using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class GameData
{
    public static int score1 = 0;
    public static int score2 = 0;
}

public class GameController : MonoBehaviour {

    private const int MAX_HP = 3;

    public GameObject player;
    private Animator playerAnim;

    private bool gameOver;
    private bool restart;

    public int itemsPerWave;
    public float startWait;
    public float spawnWait;
    public float waveWait;

    private int score;
    public GUIText scoreText;
    public GUIText restartText;
    public GUIText gameOverText;

    public GameObject fullHeartPrefab;
    private int hp;
    public GameObject[] heartSlots = new GameObject[3];
    private GameObject[] hearts = new GameObject[3];

    public ItemSpawner[] spawnPoints = new ItemSpawner[4];


    void Start()
    {
        playerAnim = player.GetComponent<Animator>();
        gameOver = false;
        restart = false;
        score = 0;
        scoreText.text = "" + score;
        restartText.text = "";
        gameOverText.text = "";
        hp = MAX_HP;
        UpdateHealthUI();

        StartCoroutine (SpawnWaves());
    }

    void Update()
    {
        if (restart)
        {
            if (Input.GetKeyDown (KeyCode.Space))
            {
                SceneManager.LoadScene("Game01");
            }
        }

        if (!gameOver)
        {
            Score(1);
        }
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
                yield return new WaitForSeconds(spawnWait);

                if (gameOver)
                {
                    restartText.text = "Press 'Space' to Restart";
                    restart = true;
                    break;
                }
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
            {
                GameObject.Destroy(hearts[i]);
            }
            UpdateHealthUIFiller(1);
        }
        else // HP 0 - He's dead, Jim
        {
            for (int i = 0; i < 3; i++ )
            {
                GameObject.Destroy(hearts[i]);
            }
        }
    }


    // Recursive function to update heart display based on HP
    void UpdateHealthUIFiller(int heart)
    {
        if (hearts[heart - 1] == null)
            hearts[heart - 1] = (GameObject) Instantiate(fullHeartPrefab, heartSlots[heart - 1].transform.position, new Quaternion());
        if (heart > 1)
            UpdateHealthUIFiller(heart - 1);
    }


    public void GameOver()
    {
        gameOver = true;
        playerAnim.SetBool("Dead", true);
        GameData.score1 = score;
        gameOverText.text = "GAME OVER";
        Destroy(player.GetComponent<PlayerController>());
    }


    public void TakeDamage( int dmg)
    {
        hp -= dmg;
        hp = Mathf.Clamp(hp, 0, 3);
        
        //Debug.Log("Damage Taken! Current HP is " + _HP);
        if (dmg == 3)
            playerAnim.SetTrigger("BombHit");
        else
            playerAnim.SetTrigger("ItemMiss");
        if (hp <= 0)
            GameOver();

        UpdateHealthUI();
    }

    public void Score( int points)
    {
        score += points;
        scoreText.text = "" + score;
    }
}
