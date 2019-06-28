using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

//Enums Idiomas
public enum Language
{
    eng,
    spa
}

public class LangManager : MonoBehaviour
{
    //Enum para saber en que idioma va a ejecutarse
    public Language selectedLanguage;

    //Diccionario de Lenguaje, que va a contener otro diccionario que va a tomar como key un ID y como valor el texto correspondiente
    public Dictionary<Language, Dictionary<string, string>> LanguageManager;

    //Url para saber desde donde descargar nuestro documento
    string externalUrl = "https://docs.google.com/spreadsheets/d/e/2PACX-1vR9b2DSZD_0tZzG17O4NHO3KmIJLvuTMLL6oWrJg75_wOMNqqGD0-LJnOJ4bSdgGMGphPsjVt5PqbYu/pub?output=csv";

    // un evento para actualizar cuando se tiene que cambiar el texto
    public event Action OnUpdate = delegate { };

    //Bajamos el Archivo de internet o lo levantamos desde el disco
    void Awake()
    {
        //LanguageManager = LanguageU.loadCodexFromString("NombreDelDoc.csv", File.ReadAllText(Application.dataPath + "/NombreDelDoc.csv"));//Para crear el ejecutable,tirar el archivo .csv dentro de la carpeta Nombre_Data   
        StartCoroutine(DownloadCSV(externalUrl));
        Debug.Log("Aca");
    }

    /// <summary>
    /// En base a una ID devolvemos la texto correspondiente
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public string GetTranslate(string _id)
    {
        if (!LanguageManager[selectedLanguage].ContainsKey(_id))
            return "Error 404: Not Found";
        else
            return LanguageManager[selectedLanguage][_id];
    }


    /// <summary>
    /// Bajamos el documento desde la pagina indicada por parametros y la cortamos 
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public IEnumerator DownloadCSV(string url)
    {
        var www = new UnityWebRequest(url);
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();    
        LanguageManager = LanguageU.loadCodexFromString("www", www.downloadHandler.text);
        OnUpdate();        
    }

    /// <summary>
    /// Funcion para guardar el documento en disco y tener un backup
    /// </summary>
    //public void ReWriteText(byte[] newText)
    //{
    //    if (File.Exists(Application.dataPath + "/NombreDelDoc.csv"))
    //    {
    //        if (File.Exists(Application.dataPath + "/NombreDelDocBackup.csv"))
    //            File.Delete(Application.dataPath + "/NombreDelDocBackup.csv");
    //        File.Copy(Application.dataPath + "/NombreDelDoc.csv", Application.dataPath + "/NombreDelDocBackup.csv");
    //        File.Delete(Application.dataPath + "/NombreDelDoc.csv");
    //        File.WriteAllBytes(Application.dataPath + "/NombreDelDoc.csv", newText);
    //    }
    //    else
    //    {
    //        File.WriteAllBytes(Application.dataPath + "/NombreDelDoc.csv", newText);
    //    }

    //}
    public void ChangeLanguage(Language ID)
    {
        selectedLanguage = ID;
        OnUpdate();
    }
}
