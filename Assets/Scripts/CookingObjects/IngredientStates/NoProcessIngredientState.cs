using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoProcessIngredientState : MonoBehaviour
{
    [SerializeField]
    private GameObject ingredientModel;

    [SerializeField]
    private int ingredientState1 = 0;
    //Solo 1 de estas a la vez
    // 0 - default

    public int ShowState1()
    {
        return ingredientState1;
    }

}
