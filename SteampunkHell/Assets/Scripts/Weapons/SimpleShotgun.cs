using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleShotgun : Weapon
{
    public override void Reload()
    {
        base.Reload();
        if (_reloading) return;
        _reloading = true;
        base.Reload();
        _anim.speed = reloadSpeed;
        StartCoroutine(ReloadTimer(reloadSpeed));
    }

    protected virtual IEnumerator ReloadTimer(float reloadSpeed)
    {
        _anim.SetTrigger("Reload");
        yield return new WaitForSeconds(reloadSpeed);
        reloadAmount = _totalReload;
        _reloading = false;
    }
}
