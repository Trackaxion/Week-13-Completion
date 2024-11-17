using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject player;
    public GameObject enemy;
    public GameObject cloud;
    public GameObject powerUp;
    private bool isPlayerAlive;
    public GameObject coin;
    private int score;
    private int life;
    public int cloudSpeed;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI restartText;
    public TextMeshProUGUI powerupText;
    //Added AudioClips by Isaiah Lopez
    public AudioClip powerup;
    public AudioClip powerDown;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(player, transform.position, Quaternion.identity);
        InvokeRepeating("CreateEnemy", 1f, 3f);
        InvokeRepeating("CreateCoin", 1f, 3f);
        StartCoroutine(CreatePowerup());
        CreateSky();
        score = 0;
        scoreText.text = "Score: " + score;
        life = 3;
        lifeText.text = "Life: " + life;
        isPlayerAlive = true;
        cloudSpeed = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && isPlayerAlive == false)
        {
            SceneManager.LoadScene("Game");
        }
    }

    void CreateEnemy()
    {
        Instantiate(enemy, new Vector3(Random.Range(-9f, 9f), 7.5f, 0), Quaternion.identity);
    }

    IEnumerator CreatePowerup()
    {
        Instantiate(powerUp, new Vector3(Random.Range(-9f, 9f), 7.5f, 0), Quaternion.identity);
        yield return new WaitForSeconds(Random.Range(3f, 6f));
        StartCoroutine(CreatePowerup());
    }

    void CreateSky()
    {
        for (int i = 0; i < 30; i++)
        {
            Instantiate(cloud, transform.position, Quaternion.identity);
        }
    }

    public void EarnScore(int howMuch)
    {
        score = score + howMuch;
        scoreText.text = "Score: " + score;
    }

    public void LoseLife(int howMuch)
    {
        life = life - howMuch;
        lifeText.text = "Life: " + life;
    }

    
    void CreateCoin()
    {
        Instantiate(coin, new Vector3(Random.Range(-9f, 9f), 7.5f, 0), Quaternion.identity);
    }

    public void GameOver()
    {
        isPlayerAlive = false;
        CancelInvoke();
        gameOverText.gameObject.SetActive(true);
        restartText.gameObject.SetActive(true);
        cloudSpeed = 0;

    }

    public void UpdatePowerupText(string whichPowerup)
    {
        powerupText.text = whichPowerup;
    }
   
    //Added Play Audio by Isaiah Lopez
    public void PlayPowerUp()
    {
        AudioSource.PlayClipAtPoint(powerup, Camera.main.transform.position);
    }
    public void PlayPowerDown()
    {
        AudioSource.PlayClipAtPoint(powerDown, Camera.main.transform.position);
    }
}
