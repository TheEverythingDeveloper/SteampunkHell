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
    public Color adrenalinBarNormalColor;
    public Color adrenalinBarCanUltiColor;
    public Color[] adrenalineBarUltiColors = new Color[3];

    private Ulti _actualUlti;
    private bool _ulting; //cuando se esta usando la ulti

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
        if (_ulting)
        {

        }
        else
        {
            ChangeCursor(CursorState.Shooting);
        }
    }

    public void StopShooting()
    {
        if (_ulting) return;

        ChangeCursor(CursorState.Normal);
    }
    
    public void ChangeAdrenalinBar(float newAdrenalin)
    {
        if (_ulting)
        {
            adrenalinBar.color = adrenalineBarUltiColors[(int)_actualUlti];
        }
        else
        {
            if (newAdrenalin >= 99)
            {
                adrenalinBar.color = adrenalinBarCanUltiColor;
                SwitchCanUlti(true);
            }
            else
            {
                adrenalinBar.color = adrenalinBarNormalColor;
                SwitchCanUlti(false);
            }
        }
        adrenalinBar.fillAmount = newAdrenalin / 100;
    }

    public void NewPoints(float points)
    {
        //TODO: Funcion de puntos apareciendo
    }

    public void SwitchCanUlti(bool on)
    {
        firstUltiImage.gameObject.SetActive(on);
        secondUltiImage.gameObject.SetActive(on);
        thirdUltiImage.gameObject.SetActive(on);
    }

    public void UltiActivation(Ulti ultiType, bool on)
    {
        _actualUlti = ultiType;
        _ulting = on;
        if (on)
        {
            switch (ultiType)
            {
                case Ulti.AGRESSIVE:
                    cursorRotationSpeed = 4f;
                    break;
                case Ulti.PRECISION:
                    cursorRotationSpeed = -10f;
                    break;
                case Ulti.REWIND:
                    cursorRotationSpeed = 1.5f;
                    break;
            }
        }
        else
        {
            cursorRotationSpeed = 0f;
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
}