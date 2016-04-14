using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    private const int MAX_HP = 3;

    public GameObject player;

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
    private int _HP;
    public GameObject[] heartSlots = new GameObject[3];
    private GameObject[] hearts = new GameObject[3];

    public ItemSpawner[] spawnPoints = new ItemSpawner[4];
    

    void Start()
    {
        gameOver = false;
        restart = false;
        score = 0;
        scoreText.text = "" + score;
        restartText.text = "";
        gameOverText.text = "";
        _HP = MAX_HP;
        UpdateHealth();

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
                else if (rngValue < 60)
                    itemIndex = 1;
                else if (rngValue < 70)
                    itemIndex = 2;
                else
                    itemIndex = 3;

                spawnPoints[spawnIndex].SpawnItem(itemIndex);
                //Debug.Log("Spawn attempt made at point " + spawnIndex);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);

            if (gameOver)
            {
                restartText.text = "Press 'Space' to Restart";
                restart = true;
                break;
            }
        }

        yield return new WaitForSeconds(startWait);
    }


    void UpdateHealth()
    {
        if (_HP == 3)
            UpdateHealthFiller(3);
        else if (_HP == 2)
        {
            GameObject.Destroy(hearts[2]);
            UpdateHealthFiller(2);
        }
        else if (_HP == 1)
        {
            for (int i = 1; i < 3; i++)
            {
                GameObject.Destroy(hearts[i]);
            }
            UpdateHealthFiller(1);
        }
        else // HP 0 - He's dead, Jim
        {
            for (int i = 0; i < 3; i++ )
            {
                GameObject.Destroy(hearts[i]);
            }
            GameOver();
        }
    }


    // Recursive function to update heart display based on HP
    void UpdateHealthFiller(int heart)
    {
        if (hearts[heart - 1] == null)
            hearts[heart - 1] = (GameObject) Instantiate(fullHeartPrefab, heartSlots[heart - 1].transform.position, new Quaternion());
        if (heart > 1)
            UpdateHealthFiller(heart - 1);
    }


    public void GameOver()
    {
        gameOver = true;
        gameOverText.text = "GAME OVER";
        Destroy(player);
    }


    public void TakeDamage()
    {
        _HP -= 1;
        _HP = Mathf.Clamp(_HP, 0, 3);
        UpdateHealth();
        //Debug.Log("Damage Taken! Current HP is " + _HP);
    }

    public void Score( int points)
    {
        score += points;
        scoreText.text = "" + score;
    }
}
