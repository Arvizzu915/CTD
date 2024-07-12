using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseIngredientScript : MonoBehaviour
{
    //Esto es lo que se me hace HORRIBLE, pero no se me ocurre de otra... o quiza si?
    [SerializeField]
    private GameObject[] ingredientModels00;
    // 0 - default, default, default
    // 1 - default, default, cocido
    // 2 - default, default, freido
    // 3 - default, default, quemado
    private GameObject[] ingredientModels01;
    // 0 - default, empanizado, default
    // 1 - default, empanizado, cocido
    // 2 - default, empanizado, freido
    // 3 - default, empanizado, quemado
    private GameObject[] ingredientModels10;
    // 0 - cortado, default, default
    // 1 - cortado, default, cocido
    // 2 - cortado, default, freido
    // 3 - cortado, default, quemado
    private GameObject[] ingredientModels11;
    // 0 - cortado, empanizado, default
    // 1 - cortado, empanizado, cocido
    // 2 - cortado, empanizado, freido
    // 3 - cortado, empanizado, quemado
    private GameObject lastModel;

    [SerializeField]
    private int ingredientState1 = 0;
    private int ingredientState2 = 0;
    private int ingredientState3 = 0;
    //State 1
    // 0 - default / completo
    // 1 - cortado
    //State 2
    // 0 - default
    // 1 - empanizado
    //State 3
    // 0 - default / crudo
    // 1 - cocido/cocinado (en olla)
    // 2 - freido
    // 3 - quemado (este quiza no haga falta, pero lo dejare por si hay algun ingrediente que se pueda meter a un horno sin nada)

    [SerializeField]
    private int[] _000MixIDs, _001MixIDs, _012MixIDs, _100MixIDs, _102MixIDs, _112MixIDs;
    private int[] currentMixIDs = new int[0];

    [SerializeField]
    private int _001MainContainerID;
    private int currentMainContainerID = 0;

    [SerializeField]
    private int _000ChangeState2ID, _100ChangeState2ID;
    private int currentChangeState2ID = 0;
    [SerializeField]
    private bool isState2Changer;

    public void ChangeState1(int newState)
    {
        ingredientState1 = newState;
        lastModel.SetActive(false);
        ShowModel();
        ChangeAttributesIDs();
    }

    public void ChangeState2(int newState)
    {
        ingredientState2 = newState;
        lastModel.SetActive(false);
        ShowModel();
        ChangeAttributesIDs();
    }

    public void ChangeState3(int newState)
    {
        ingredientState3 = newState;
        lastModel.SetActive(false);
        ShowModel();
        ChangeAttributesIDs();
    }

    private void ChangeAttributesIDs()
    {
        if (ingredientState1 == 0 && ingredientState2 == 0 && ingredientState3 == 0)
        {
            currentMixIDs = _000MixIDs;
            currentMainContainerID = 0;
            currentChangeState2ID = _000ChangeState2ID;
        }
        else if (ingredientState1 == 0 && ingredientState2 == 0 && ingredientState3 == 1)
        {
            currentMixIDs = _001MixIDs;
            currentMainContainerID = _001MainContainerID;
            currentChangeState2ID = 0;
        }
        else if (ingredientState1 == 0 && ingredientState2 == 1 && ingredientState3 == 2)
        {
            currentMixIDs = _012MixIDs;
            currentMainContainerID = 0;
            currentChangeState2ID = 0;
        }
        else if (ingredientState1 == 1 && ingredientState2 == 0 && ingredientState3 == 0)
        {
            currentMixIDs = _100MixIDs;
            currentMainContainerID = 0;
            currentChangeState2ID = _100ChangeState2ID;
        }
        else if (ingredientState1 == 1 && ingredientState2 == 0 && ingredientState3 == 2)
        {
            currentMixIDs = _102MixIDs;
            currentMainContainerID = 0;
            currentChangeState2ID = 0;
        }
        else if (ingredientState1 == 1 && ingredientState2 == 1 && ingredientState3 == 2)
        {
            currentMixIDs = _112MixIDs;
            currentMainContainerID = 0;
            currentChangeState2ID = 0;
        }
        else
        {
            currentMixIDs = new int[0];
            currentMainContainerID = 0;
            currentChangeState2ID = 0;
        }
    }

    public int ShowState1()
    {
        return ingredientState1;
    }

    public int ShowState2()
    {
        return ingredientState2;
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

    public int ShowChangeState2ID()
    {
        return currentChangeState2ID;
    }

    public bool ShowIfIsState2Changer()
    {
        return isState2Changer;
    }

    private void ShowModel()
    {
        switch (ingredientState1)
        {
            case 0:
                switch (ingredientState2)
                {
                    case 0:
                        ingredientModels00[ingredientState3].SetActive(true);
                        lastModel = ingredientModels00[ingredientState3];
                        break;
                    case 1:
                        ingredientModels01[ingredientState3].SetActive(true);
                        lastModel = ingredientModels01[ingredientState3];
                        break;
                }
                break;
            case 1:
                switch (ingredientState2)
                {
                    case 0:
                        ingredientModels10[ingredientState3].SetActive(true);
                        lastModel = ingredientModels10[ingredientState3];
                        break;
                    case 1:
                        ingredientModels11[ingredientState3].SetActive(true);
                        lastModel = ingredientModels11[ingredientState3];
                        break;
                }
                break;
        }
    }
}
