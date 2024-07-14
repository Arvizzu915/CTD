using UnityEngine;

public interface ITurretState
{
    void OnAttack();
    void LookForTarget();
    void UpdateState();
    bool CanUpgradeTurret();
}
