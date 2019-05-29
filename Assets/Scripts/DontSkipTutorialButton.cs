using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DontSkipTutorialButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
    [SerializeField]
    private TextMeshProUGUI buttonText;
    [SerializeField]
    private Image buttonImage;
    [SerializeField]
    private GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        buttonImage.color = Color.white;
        gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gm == null)
        {
            gm = FindObjectOfType<GameManager>();
        }
    }

    public void OnPointerDown(PointerEventData evt)
    {
        buttonImage.color = Color.gray;
        gm.DontSkipTutorial();
    }

    public void OnPointerEnter(PointerEventData evt)
    {
        buttonImage.color = Color.gray;
        gm.DontSkipTutorial();
    }
}
