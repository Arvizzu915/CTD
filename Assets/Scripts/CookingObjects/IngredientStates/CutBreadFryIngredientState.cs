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
    private int _012MixID, _100MixID, _112MixID;
    private int currentMixID = -1;

    public void ChangeState1(int newState)
    {
        ingredientState1 = newState;
        lastModel.SetActive(false);
        ShowModel();
        ChangeMixID();
    }

    public void ChangeState2(int newState)
    {
        ingredientState2 = newState;
        lastModel.SetActive(false);
        ShowModel();
        ChangeMixID();
    }

    public void ChangeState3(int newState)
    {
        ingredientState3 = newState;
        lastModel.SetActive(false);
        ShowModel();
        ChangeMixID();
    }

    private void ChangeMixID()
    {
        if (ingredientState1 == 0 && ingredientState2 == 1 && ingredientState3 == 2)
        {
            currentMixID = _012MixID;
        }
        else if (ingredientState1 == 1 && ingredientState2 == 0 && ingredientState3 == 0)
        {
            currentMixID = _100MixID;
        }
        else if (ingredientState1 == 1 && ingredientState2 == 1 && ingredientState3 == 2)
        {
            currentMixID = _112MixID;
        }
        else
        {
            currentMixID = -1;
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

    public int ShowMixID()
    {
        return currentMixID;
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
