using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutBreadFryIngredientState : MonoBehaviour
{
    [SerializeField]
    private GameObject[] ingredientModels00;
    // 0 - default, default, default
    // 1 - default, default, freido
    private GameObject[] ingredientModels01;
    // 0 - default, empanizado, default
    // 1 - default, empanizado, freido
    private GameObject[] ingredientModels10;
    // 0 - cortado, default, default
    // 1 - cortado, default, freido
    private GameObject[] ingredientModels11;
    // 0 - cortado, empanizado, default
    // 1 - cortado, empanizado, freido
    private GameObject lastModel;

    [SerializeField]
    private int ingredientState1 = 0;
    private int ingredientState2 = 0;
    private int ingredientState3 = 0;

    //Solo 1 de estas a la vez
    // 0 - default / completo
    // 1 - cortado

    //Solo 1 de estas a la vez
    // 0 - default
    // 1 - empanizado

    //Solo 1 de estas a la vez
    // 0 - default
    // 2 - freido

    [SerializeField]
    private int[] _012MixIDs, _100MixIDs, _112MixIDs;
    private int[] currentMixIDs = new int[0];

    [SerializeField]
    private int _000ChangeState2ID, _100ChangeState2ID;
    private int currentChangeState2ID = 0;

    public void ChangeState1(int newState)
    {
        ingredientState1 = newState;
        lastModel.SetActive(false);
        ShowModel();
        ChangeMixIDs();
        ChangeChangeState2ID();
    }

    public void ChangeState2(int newState)
    {
        ingredientState2 = newState;
        lastModel.SetActive(false);
        ShowModel();
        ChangeMixIDs();
        ChangeChangeState2ID();
    }

    public void ChangeState3(int newState)
    {
        ingredientState3 = newState;
        lastModel.SetActive(false);
        ShowModel();
        ChangeMixIDs();
        ChangeChangeState2ID();
    }

    private void ChangeMixIDs()
    {
        if (ingredientState1 == 0 && ingredientState2 == 1 && ingredientState3 == 2)
        {
            currentMixIDs = _012MixIDs;
        }
        else if (ingredientState1 == 1 && ingredientState2 == 0 && ingredientState3 == 0)
        {
            currentMixIDs = _100MixIDs;
        }
        else if (ingredientState1 == 1 && ingredientState2 == 1 && ingredientState3 == 2)
        {
            currentMixIDs = _112MixIDs;
        }
        else
        {
            currentMixIDs = new int[0];
        }
    }

    private void ChangeChangeState2ID()
    {
        if (ingredientState1 == 0 && ingredientState2 == 0 && ingredientState3 == 0)
        {
            currentChangeState2ID = _000ChangeState2ID;
        }
        else if (ingredientState1 == 1 && ingredientState2 == 0 && ingredientState3 == 0)
        {
            currentChangeState2ID = _100ChangeState2ID;
        }
        else
        {
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

    public int ShowChangeState2ID()
    {
        return currentChangeState2ID;
    }

    private void ShowModel()
    {
        switch (ingredientState1)
        {
            case 0:
                switch (ingredientState2)
                {
                    case 0:
                        switch (ingredientState3)
                        {
                            case 0:
                                ingredientModels00[0].SetActive(true);
                                lastModel = ingredientModels00[0];
                                break;
                            case 1:
                                ingredientModels00[1].SetActive(true);
                                lastModel = ingredientModels00[1];
                                break;
                        }
                        break;
                    case 1:
                        switch (ingredientState3)
                        {
                            case 0:
                                ingredientModels01[0].SetActive(true);
                                lastModel = ingredientModels01[0];
                                break;
                            case 1:
                                ingredientModels01[1].SetActive(true);
                                lastModel = ingredientModels01[1];
                                break;
                        }
                        break;
                }
                break;
            case 1:
                switch (ingredientState2)
                {
                    case 0:
                        switch (ingredientState3)
                        {
                            case 0:
                                ingredientModels10[0].SetActive(true);
                                lastModel = ingredientModels10[0];
                                break;
                            case 1:
                                ingredientModels10[1].SetActive(true);
                                lastModel = ingredientModels10[1];
                                break;
                        }
                        break;
                    case 1:
                        switch (ingredientState3)
                        {
                            case 0:
                                ingredientModels11[0].SetActive(true);
                                lastModel = ingredientModels11[0];
                                break;
                            case 1:
                                ingredientModels11[1].SetActive(true);
                                lastModel = ingredientModels11[1];
                                break;
                        }
                        break;
                }
                break;
        }
    }
}
