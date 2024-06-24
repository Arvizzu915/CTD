using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret1State : ITurretState
{
    //Estas funciones nomas estan de mientras, luego se cambiaran para ver que funciona mejor pal tower defense
    private float bulletDmg;
    private float fireRate;
    int upgradeCount = 0;

    public Turret1State(float bulletDmg, float fireRate)
    {
        this.bulletDmg = bulletDmg;
        this.fireRate = fireRate;
    }

    public void LookForTarget()
    {
        throw new System.NotImplementedException();
    }

    public void OnAttack()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateState()
    {
        throw new System.NotImplementedException();
    }

    public bool CanUpgradeTurret()
    {
        if (upgradeCount < 2)
        {
            upgradeCount++;
            UpgradeTower(upgradeCount);
            return true;
        }
        Debug.Log("Ya no se puede mejorar, ya estas al maximo");
        return false;
    }

    private void UpgradeTower(int upgradeLevel)
    {
        if(upgradeLevel == 1)
        {
            //esto es mejorar la torre a nivel 2
            bulletDmg = 6f;
            fireRate = 0.9f;
            Debug.Log("Ahora soy nivel 2 B)");
        }
        else if(upgradeLevel == 2)
        {
            //Esto es mejroar la torre al maximo wuuuu
            bulletDmg = 666f;
            fireRate = 0.1f;
            Debug.Log("Estoy mamadisimo (soy nivel maximo)");
        }
    }
}
