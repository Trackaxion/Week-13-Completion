using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{

    private float horizontalInput;
    private float verticalInput;
    private float speed;
    private float horizontalScreenLimit;
    private float verticalScreenLimit;
    public GameObject explosion;
    public GameObject bullet;
    public GameObject thruster;
    public GameObject shield;
    private int lives;
    private int shooting;
    public GameManager gameManager;
    private bool hasShield;

    // Start is called before the first frame update
    void Start()
    {
        speed = 6f;
        horizontalScreenLimit = 11.5f;
        verticalScreenLimit = 7.5f;
        lives = 3;
        shooting = 1;
        hasShield = false;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        thruster = GameObject.Find("Thruster").gameObject;
        shield = GameObject.Find("Shield").gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Shooting();
    }
    
    void Movement()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * Time.deltaTime * speed);
        if (transform.position.x > horizontalScreenLimit || transform.position.x <= -horizontalScreenLimit)
        {
            transform.position = new Vector3(transform.position.x * -1, transform.position.y, 0);
        }
        if (transform.position.y > verticalScreenLimit || transform.position.y <= -verticalScreenLimit)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y * -1, 0);
        }
    }

    void Shooting()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (shooting)
            {
                case 1:
                        Instantiate(bullet, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                    break;
                case 2:
                        Instantiate(bullet, transform.position + new Vector3(0.5f, 1, 0), Quaternion.identity);
                        Instantiate(bullet, transform.position + new Vector3(-0.5f, 1, 0), Quaternion.identity);
                    break;
                case 3:
                        Instantiate(bullet, transform.position + new Vector3(0.5f, 1, 0), Quaternion.Euler(0, 0, -30f));
                        Instantiate(bullet, transform.position + new Vector3(-0.5f, 1, 0), Quaternion.Euler(0, 0, 30f));
                        Instantiate(bullet, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                    break;
            }
        }
    }

    public void LoseALife()
    {
        if (hasShield == false)
        {
            lives--;
        } 
        else if (hasShield == true)
        {
            //Shield by Donovan Peckham
            shield.gameObject.SetActive(false);
            GameObject.Find("GameManager").GetComponent<GameManager>().LoseLife(-1);
            hasShield = false;
        }
        if (lives == 0)
        {
            gameManager.GameOver();
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    IEnumerator SpeedPowerDown()
    {
        yield return new WaitForSeconds(3f);
        speed = 6f;
        thruster.gameObject.SetActive(false);
        gameManager.UpdatePowerupText("");
    }

    IEnumerator ShootingPowerDown()
    {
        yield return new WaitForSeconds(4f);
        shooting = 1;
        gameManager.UpdatePowerupText("");
    }

    IEnumerator ShieldPowerDown()
    {
        yield return new WaitForSeconds(3f);
        gameManager.UpdatePowerupText("");
    }

    private void OnTriggerEnter2D(Collider2D whatIHit)
    {
        if(whatIHit.tag == "Powerup")
        {
            int powerupType = Random.Range(1, 5);
            switch(powerupType)
            {
                case 1:
                    //speed
                    speed = 9f;
                    gameManager.UpdatePowerupText("Picked up Speed!");
                    thruster.gameObject.SetActive(true);
                    StartCoroutine(SpeedPowerDown());
                    break;
                case 2:
                    //double shot
                    shooting = 2;
                    gameManager.UpdatePowerupText("Picked up Double Shot!");
                    StartCoroutine(ShootingPowerDown());
                    break;
                case 3:
                    //triple shot
                    shooting = 3;
                    gameManager.UpdatePowerupText("Picked up Triple Shot!");
                    StartCoroutine(ShootingPowerDown());
                    break;
                case 4:
                    //Shield by Donovan Peckham
                    hasShield = true;
                    shield.gameObject.SetActive(true);
                    gameManager.UpdatePowerupText("Picked up Shield!");
                    break;
            }
            Destroy(whatIHit.gameObject);
        }
    }
}