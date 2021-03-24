using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpGenerator : MonoBehaviour
{
    public static PopUpGenerator sharedInstance { get; set; }

    public Camera mainCamera;
    public Canvas mainCanvas;

    public TextMeshProUGUI          PopUpText_Prefab;

    public Queue<TextMeshProUGUI>   InScenePopUpQueue;

    private TextMeshProUGUI currentPopUp;

    public float PopUpSpeed = 500f;
    public bool  isPoppingUp;

    public float TIMER = 0.25f;

    private void Awake()
    {
        sharedInstance = this;
    }

    private void Start()
    {
        InScenePopUpQueue = new Queue<TextMeshProUGUI>();
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        mainCanvas = GameObject.FindWithTag("Canvas").GetComponent<Canvas>();
    }

    private void Update()
    {
        UpdatePopAnimation();

        if (InScenePopUpQueue.Count != 0)
        {
            TIMER -= Time.deltaTime;

            if(TIMER <= 0f)
            {
                Destroy(InScenePopUpQueue.Dequeue().gameObject);
                TIMER = 0.25f;
            }
        }
    }

    public void PopUpScore(Vector3 _coinPosition)
    {
        isPoppingUp = true;

        currentPopUp = Instantiate(PopUpText_Prefab, GetCanvasPosition(_coinPosition), Quaternion.identity, mainCanvas.transform);

        InScenePopUpQueue.Enqueue(currentPopUp);
    }

    public void UpdatePopAnimation()
    {
        if (isPoppingUp)
        {
            foreach(TextMeshProUGUI text in InScenePopUpQueue)
            {
                text.GetComponent<RectTransform>().anchoredPosition += Vector2.up * PopUpSpeed * Time.deltaTime;
            }
        }
    }

    private Vector2 GetCanvasPosition(Vector3 _coinPosition)
    {
        Vector2 ViewportPosition = mainCamera.WorldToScreenPoint(_coinPosition);
        return ViewportPosition;
    }
}
