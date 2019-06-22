using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Weapon[] allWeapons;
    public Weapon actualWeapon;

    private void Awake()
    {
        allWeapons = GetComponentsInChildren<Weapon>();
        ChangeWeapon(0);
    }

    public void ChangeWeapon(int weaponID)
    {
        foreach (var item in allWeapons)
        {
            item.gameObject.SetActive(false);
        }

        allWeapons[weaponID].gameObject.SetActive(true);
        actualWeapon = allWeapons[weaponID];
    }
}
