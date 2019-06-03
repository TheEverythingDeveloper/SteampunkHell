using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextTranslate : MonoBehaviour
{
    public string ID;
    [HideInInspector] public LangManager manager;
    TextMeshProUGUI myView;

    private void Start()
    {
        myView = GetComponent<TextMeshProUGUI>();
        manager.OnUpdate += ChangeLang;
    }

    void ChangeLang()
    {
        myView.text= manager.GetTranslate(ID);
    }
}
