using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTurretScript : MonoBehaviour
{
    private enum TurretTypes { Turret1, Turret2, Turret3 }
    [SerializeField]
    private TurretTypes turretType = TurretTypes.Turret1;

    ITurretState turretState;

    void Start()
    {
        switch (turretType)
        {
            case TurretTypes.Turret1:
                turretState = new Turret1State(5f, 1f);
                break;
            case TurretTypes.Turret2:
                turretState = new Turret1State(1f, 0.2f);
                break;
            case TurretTypes.Turret3:
                turretState = new Turret1State(20f, 5f);
                break;
        }
    }

    public void OnAttack()
    {
        throw new System.NotImplementedException();
    }

    public void LookForTarget()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateState()
    {
        throw new System.NotImplementedException();
    }

    public bool CanUpgradeTurret()
    {
        return turretState.CanUpgradeTurret();
    }
}
