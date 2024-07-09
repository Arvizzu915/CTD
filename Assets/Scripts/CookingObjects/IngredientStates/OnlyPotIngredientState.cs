using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyPotIngredientState : MonoBehaviour
{
    [SerializeField]
    private GameObject[] ingredientModels00;
    // 0 - default
    // 1 - cocido
    private GameObject lastModel;

    [SerializeField]
    private int ingredientState3 = 0;
    //Solo 1 de estas a la vez
    // 0 - default
    // 1 - cocido/cocinado (en olla)

    [SerializeField]
    private int _001MixID;
    private int currentMixID = -1;

    public void ChangeState3(int newState)
    {
        ingredientState3 = newState;
        lastModel.SetActive(false);
        ingredientModels00[ingredientState3].SetActive(true);
        lastModel = ingredientModels00[ingredientState3];
        ChangeMixID();
    }

    private void ChangeMixID()
    {
        if (ingredientState3 == 1)
        {
            currentMixID = _001MixID;
        }
        else
        {
            currentMixID = -1;
        }
    }

    public int ShowState3()
    {
        return ingredientState3;
    }

    public int ShowMixID()
    {
        return currentMixID;
    }
}
