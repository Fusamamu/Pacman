using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PassPositionEvent : UnityEvent<Vector3>
{
}

public class Coin : MonoBehaviour
{
    public enum CoinType
    {
        SMALL_COIN,
        BIG_COIN
    }

    public CoinType type;

    public UnityEvent OnEnergizerCollected;
    public UnityEvent OnCoinCollected;
    public PassPositionEvent OnCoinDestroyed;

    private List<Ghost> allGhosts;

    private void Start()
    {
        allGhosts = GetAllObjectsOnlyInScene();

        foreach(Ghost ghost in allGhosts)
        {
            OnEnergizerCollected.AddListener(ghost.AvoidPlayer);
        }
        
        OnCoinCollected.AddListener(AudioManager.sharedInstance.OnEating);
        OnCoinCollected.AddListener(ScoreManager.sharedInstance.OnSmallCoinCollected);

        OnCoinDestroyed.AddListener(PopUpGenerator.sharedInstance.PopUpScore);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            switch (type)
            {
                case CoinType.SMALL_COIN:
                    break;
                case CoinType.BIG_COIN:
                    OnEnergizerCollected.Invoke();
                    break;
            }

            OnCoinCollected.Invoke();
            OnCoinDestroyed.Invoke(transform.position);

            Destroy(this.gameObject);
            //gameObject.SetActive(false);
        }
    }

    List<Ghost> GetAllObjectsOnlyInScene()
    {
        List<Ghost> objectsInScene = new List<Ghost>();

        foreach (Ghost go in Resources.FindObjectsOfTypeAll(typeof(Ghost)) as Ghost[])
        {
            if (!EditorUtility.IsPersistent(go.transform.root.gameObject) && !(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave))
                objectsInScene.Add(go);
        }

        return objectsInScene;
    }
}
