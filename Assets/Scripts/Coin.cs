using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public enum CoinType
    {
        SMALL_COIN,
        BIG_COIN
    }

    public CoinType type;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Coin Destroyed");
            Destroy(this.gameObject);
        }
    }
}
