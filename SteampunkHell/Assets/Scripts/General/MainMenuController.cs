using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(LangManager))]
public class MainMenuController : MonoBehaviour
{
    public List<TextTranslate> translations = new List<TextTranslate>();
    public GameObject mainStep;
    public GameObject optionsStep;
    LangManager langManager;

    private void Awake()
    {
        langManager = GetComponent<LangManager>();
        optionsStep.SetActive(false);
        foreach (var item in translations)
        {
            item.manager = langManager;
        }
    }

    #region MainStep
    public void PlayButton()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void OptionsButton()
    {
        Debug.Log("abriendo configuracion");
        ChangeStep();
    }

    public void ExitButton()
    {
        Debug.Log("exit game");
        Application.Quit();
    }
    #endregion

    #region OptionsStep
    public void ChangeLanguage(int ID)
    {
        langManager.ChangeLanguage((Language)ID);
    }

    public void BackButton()
    {
        Debug.Log("back to main step");
        ChangeStep();
    }
    #endregion

    private void ChangeStep()
    {
        mainStep.SetActive(!mainStep.activeSelf);
        optionsStep.SetActive(!optionsStep.activeSelf);
    }
}
