using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAdrenalinController : MonoBehaviour
{
    public float adrenalin;
    public float adrenalinLoss = 5;
    public float adrenalinLossTimeMultiplier = 0.8f;
    private float _initialAdrenalinLoss;
    public float adrenalinLossTime = 2f;
    public float adrenalinLossCDInUlti = 10f;
    public float ultiLossSpeed = 0.5f;

    [Tooltip("0 = Laser, 1 = Heavy, 2 = Explosive, 3 = Zeppellin, 4 = Boss, 5 = Other")]
    public List<float> adrenalinTable;

    private Model _model;

    private void Awake()
    {
        _model = GetComponent<Model>();
    }

    private void Start()
    {
        _initialAdrenalinLoss = adrenalinLossTime;
        ChangeAdrenalin(0);
        StartCoroutine(AdrenalinLossCoroutine());
    }

    public void KillEnemy(Unit unitID)
    {
        ChangeAdrenalin(adrenalinTable[(int)unitID]);
    }

    private void ChangeAdrenalin(float amount)
    {
        if (adrenalin + amount > adrenalin) adrenalinLossTime = _initialAdrenalinLoss;
        else adrenalinLossTime *= adrenalinLossTimeMultiplier;

        adrenalinLossTime = Mathf.Clamp(adrenalinLossTime, 0.3f, 20f);
        adrenalin += amount;
        adrenalin = Mathf.Clamp(adrenalin, 0, 100);
        UIController.Instance.ChangeAdrenalinBar(adrenalin);
    }

    public void UltiActivation(Ulti ultiID, bool on)
    {
        if (on)
        {
            StopCoroutine(AdrenalinLossCoroutine());
            StartCoroutine(AdrenalinUltiLossCoroutine(ultiID));
        }
        else
        {
            StartCoroutine(AdrenalinLossCoroutine());
            StopCoroutine(AdrenalinUltiLossCoroutine(ultiID));
        }
    }


    IEnumerator AdrenalinUltiLossCoroutine(Ulti activeUlti) //Perdida de adrenalina con la ulti activada
    {
        while(adrenalin > 0)
        {
            ChangeAdrenalin(Time.deltaTime * ultiLossSpeed);
            yield return new WaitForEndOfFrame();
        }
        _model.UltiActivation(activeUlti);
    }

    IEnumerator AdrenalinLossCoroutine() //Perdida de adrenalina natural
    {
        yield return new WaitForSeconds(adrenalinLossTime);
        if(adrenalin >= 99)
        {
            yield return new WaitForSeconds(adrenalinLossCDInUlti);
        }
        ChangeAdrenalin(-adrenalinLoss);
        StartCoroutine(AdrenalinLossCoroutine());
    }
    public bool CheckUltiActivation()
    {
        return adrenalin > 99;
    }
}
