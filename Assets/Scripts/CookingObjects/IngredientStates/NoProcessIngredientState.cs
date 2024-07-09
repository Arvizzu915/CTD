using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoProcessIngredientState : MonoBehaviour
{
    [SerializeField]
    private GameObject ingredientModel;

    [SerializeField]
    private int _000MixID;

    public int ShowMixID()
    {
        return _000MixID;
    }
}
