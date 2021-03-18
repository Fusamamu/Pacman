using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Coin : MonoBehaviour
{
    public enum CoinType
    {
        SMALL_COIN,
        BIG_COIN
    }

    public CoinType type;

    public UnityEvent OnBigCoinDestroyed;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            switch (type)
            {
                case CoinType.SMALL_COIN:
                    break;
                case CoinType.BIG_COIN:
                    OnBigCoinDestroyed.Invoke();
                    break;
            }

            Debug.Log("Coin Destroyed");
            Destroy(this.gameObject);


        }
    }
}
