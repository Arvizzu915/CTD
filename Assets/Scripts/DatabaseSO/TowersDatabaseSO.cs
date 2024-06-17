using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Towers Database SO", menuName = "Towers Database SO")]
public class TowersDatabaseSO : ScriptableObject
{
    public List<TurretData> turretsData;
}

[Serializable]
public class TurretData
{
    [field: SerializeField]
    public string Name { get; private set; }
    [field: SerializeField]
    public int ID { get; private set; }
    [field: SerializeField]
    public float Damage { get; private set; }
    [field: SerializeField]
    public float Firerate { get; private set; }
    [field: SerializeField]
    public float Range { get; private set; }
    [field: SerializeField]
    public bool HasInvisibleDetection { get; private set; }
    [field: SerializeField]
    public bool HasFlyingDetection { get; private set; }
}