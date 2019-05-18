using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    public Image lifeBar;
    public Image hurtVignette;
    [HideInInspector]
    public float previousHP;
    [HideInInspector]
    public float totalHP;

    private void Awake()
    {
        Instance = this;
    }

    public void ChangeHP(float newHP)
    {
        if(newHP < previousHP)
        {
            StopCoroutine(ActivateHurtVignette());
            StartCoroutine(ActivateHurtVignette());
        }
        previousHP = newHP;

        //actualizar barra de vida
        Debug.Log(newHP);
        lifeBar.fillAmount = newHP / totalHP;
    }

    IEnumerator ActivateHurtVignette() //Animacion de que aparezca sin animator
    {
        Debug.Log("se activo la corrutina");
        float t = 0f;
        while(t < 1)
        {
            t += Time.deltaTime;
            hurtVignette.color = new Color(hurtVignette.color.r, hurtVignette.color.g, hurtVignette.color.b, t);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(1f);
        t = 1f;
        while (t > 0)
        {
            t -= Time.deltaTime / 4f;
            hurtVignette.color = new Color(hurtVignette.color.r, hurtVignette.color.g, hurtVignette.color.b, t);
            yield return new WaitForEndOfFrame();
        }
    }
}
