using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    public Image lifeBar;
    public Image adrenalinBar;
    public Image hurtVignette;
    [HideInInspector]
    public float previousHP;
    [HideInInspector]
    public float totalHP;

    public float cursorRotationSpeed;
    public Image cursorImage;
    public Sprite normalCursorSprite;
    public Sprite shootingCursorSprite;
    public Color normalColor;
    public Color shootingColor;

    public Image firstUltiImage, secondUltiImage, thirdUltiImage;

    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        cursorImage.transform.Rotate(Vector3.forward, cursorRotationSpeed);
    }

    public void ReceiveDamageFeedback(float life, Vector3 pushForce)
    {
        ChangeHP(life);
    }

    public void StartShooting()
    {
        ChangeCursor(CursorState.Shooting);
    }

    public void StopShooting()
    {
        //preguntar cosas como si tiene balas, o si esta viendo el enemigo
        ChangeCursor(CursorState.Normal);
    }
    
    public void ChangeAdrenalinBar(float newAdrenalin)
    {
        adrenalinBar.fillAmount = newAdrenalin / 100;
    }

    public void NewPoints(float points)
    {
        //TODO: Funcion de puntos apareciendo
    }

    public void UltiActivation(int id)
    {
        switch (id)
        {
            case 0:
                cursorRotationSpeed = 4f;
                break;
            case 1:
                cursorRotationSpeed = -10f;
                break;
            case 2:
                cursorRotationSpeed = 1.5f;
                break;
        }
    }

    private void ChangeCursor(CursorState newCursorState)
    {
        switch (newCursorState)
        {
            case CursorState.Normal:
                cursorImage.sprite = normalCursorSprite;
                cursorImage.color = normalColor;
                cursorRotationSpeed = 0f;
                break;
            case CursorState.EnemyInSight:
                cursorImage.sprite = normalCursorSprite;
                cursorImage.color = shootingColor;
                break;
            case CursorState.Shooting:
                cursorImage.sprite = shootingCursorSprite;
                cursorImage.color = shootingColor;
                cursorRotationSpeed = -4f;
                break;
            case CursorState.CantShoot:
                cursorImage.sprite = shootingCursorSprite;
                cursorImage.color = shootingColor;
                break;
        }
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

public enum CursorState
{
    Normal,
    EnemyInSight,
    Shooting,
    CantShoot,
}