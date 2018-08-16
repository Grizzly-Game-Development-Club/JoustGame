using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg_Controller : MonoBehaviour {

    int CoinValue = 100;

    void Update()
    {
        Vector2 lowerLeft = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        if (this.transform.position.y <= lowerLeft.y) {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameObject.Find("Game Manager").GetComponent<Game_Manager>().addPlayerScore(CoinValue, collision.gameObject.GetComponent<Player_Controller>().PlayerID);
            Destroy(this.gameObject);

        }
    }
}
