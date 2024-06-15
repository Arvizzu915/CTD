using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Cooking Objects Database SO", menuName = "Cooking Objects Database SO")]
public class CookingObjectsDatabaseSO : ScriptableObject
{
    public List<CookingObjectData> cookingObjectsData;
}

[Serializable]
public class CookingObjectData
{
    [field: SerializeField]
    public string Name { get; private set; }
    [field: SerializeField]
    public int ID { get; private set; }
    public enum Type { Plate, Base, Spices, Tool }
    [field: SerializeField]
    public Type CookingObjectType { get; private set; }
}
