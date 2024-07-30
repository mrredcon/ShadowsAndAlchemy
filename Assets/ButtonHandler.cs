using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TMP_Text theText;
    private Color darkRed;

    // Start is called before the first frame update
    void Start()
    {
        darkRed = new Color(0.4705882f, 0f, 0f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        theText.color = darkRed;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        theText.color = Color.white;
    }
}
