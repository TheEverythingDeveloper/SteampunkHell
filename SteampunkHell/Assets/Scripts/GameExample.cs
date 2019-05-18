using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameExample : MonoBehaviour 
{
    public float heroLife;
    public Image lifebar;

    private float _totalHeroLife;

    void Awake()
    {
        _totalHeroLife = heroLife;
        EventsManager.SubscribeToEvent(EventType.GP_HeroLifeModified, OnHeroLifeUpdated);
        EventsManager.SubscribeToEvent(EventType.GP_HeroDefeated, OnHeroDefeated);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            heroLife -= 10;
            EventsManager.TriggerEvent(EventType.GP_HeroLifeModified, new object[] { heroLife, _totalHeroLife});
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            heroLife += 10;
            EventsManager.TriggerEvent(EventType.GP_HeroLifeModified, new object[] { heroLife, _totalHeroLife });
        }

        if (heroLife <= 0)
            EventsManager.TriggerEvent(EventType.GP_HeroDefeated);
    }

    private void OnHeroDefeated(params object[] parameters)
    {
        Debug.Log("HERO DEFEATED");
        EventsManager.UnsubscribeToEvent(EventType.GP_HeroDefeated, OnHeroDefeated);
        EventsManager.UnsubscribeToEvent(EventType.GP_HeroLifeModified, OnHeroLifeUpdated);
    }

    private void OnHeroLifeUpdated(params object[] parameters)
    {
        var currentHp = (float)parameters[0];
        var totalHp = (float)parameters[1];
        lifebar.fillAmount = (currentHp/totalHp);
        Debug.Log("Actual Life: " + currentHp + "\nLife Ratio: " + (currentHp/totalHp));
    }
}