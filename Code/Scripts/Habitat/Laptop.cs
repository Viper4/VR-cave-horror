using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Laptop : MonoBehaviour
{
    bool wrapText = false;
    [SerializeField] TextMeshProUGUI[] wrappableTexts;

    public void ToggleWrap()
    {
        Debug.Log("Toggled Wrap");
        wrapText = !wrapText;
        foreach (TextMeshProUGUI text in wrappableTexts)
        {
            text.enableWordWrapping = wrapText;
            text.UpdateFontAsset();
        }
    }
}
