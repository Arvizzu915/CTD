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
    private int[] _001MixIDs;
    private int[] currentMixIDs = new int[0];

    [SerializeField]
    private int _001MainContainerID;
    private int currentMainContainerID = 0;

    public void ChangeState3(int newState)
    {
        ingredientState3 = newState;
        lastModel.SetActive(false);
        ingredientModels00[ingredientState3].SetActive(true);
        lastModel = ingredientModels00[ingredientState3];
        ChangeMixIDs();
        ChangeMainContainerID();
    }

    private void ChangeMixIDs()
    {
        if (ingredientState3 == 1)
        {
            currentMixIDs = _001MixIDs;
        }
        else
        {
            currentMixIDs = new int[0];
        }
    }

    private void ChangeMainContainerID()
    {
        if (ingredientState3 == 1)
        {
            currentMainContainerID = _001MainContainerID;
        }
        else
        {
            currentMainContainerID = 0;
        }
    }

    public int ShowState3()
    {
        return ingredientState3;
    }

    public int[] ShowMixIDs()
    {
        return currentMixIDs;
    }

    public int ShowMainContainerID()
    {
        return currentMainContainerID;
    }
}
