//Coin by Donovan Peckham
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointCoin : MonoBehaviour
{
    public GameObject coinsound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D whatDidIHit)
    {
        if (whatDidIHit.tag == "Player")
        {
            Destroy(this.gameObject);
            Instantiate(coinsound, transform.position, Quaternion.identity);
            GameObject.Find("GameManager").GetComponent<GameManager>().EarnScore(10);
        }
    }
}