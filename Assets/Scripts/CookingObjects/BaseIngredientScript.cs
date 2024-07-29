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
    [SerializeField]
    private GameObject[] ingredientModels01;
    // 0 - default, empanizado, default
    // 1 - default, empanizado, cocido
    // 2 - default, empanizado, freido
    // 3 - default, empanizado, quemado
    [SerializeField]
    private GameObject[] ingredientModels10;
    // 0 - cortado, default, default
    // 1 - cortado, default, cocido
    // 2 - cortado, default, freido
    // 3 - cortado, default, quemado
    [SerializeField]
    private GameObject[] ingredientModels11;
    // 0 - cortado, empanizado, default
    // 1 - cortado, empanizado, cocido
    // 2 - cortado, empanizado, freido
    // 3 - cortado, empanizado, quemado
    [SerializeField]
    private GameObject lastModel; //quiza haga falta un void start donde se iguale lastModel al primer modelo

    [SerializeField]
    private int ingredientState1 = 0, ingredientState2 = 0, ingredientState3 = 0;
    //State 1
    // 0 - default / completo
    // 1 - cortado
    //State 2
    // 0 - default
    // 1 - empanizado
    //State 3
    //-1 - quemado (este estado no tiene modelo propio ya que solo sirve para informar al recipiente para que este muestre el modelo quemado)
    // 0 - default / crudo
    // 1 - cocido/cocinado (en olla)
    // 2 - freido

    [SerializeField]
    private int[] _000MixIDs, _001MixIDs, _012MixIDs, _100MixIDs, _102MixIDs, _112MixIDs;
    private int[] currentMixIDs = new int[0];

    [SerializeField]
    private int[] _000ContainersAndStationsIDs = new int[1] { 100 }, _001ContainersAndStationsIDs = new int[1] { 100 }, _010ContainersAndStationsIDs = new int[1] { 100 };
    [SerializeField]
    private int[] _100ContainersAndStationsIDs = new int[1] { 100 }, _110ContainersAndStationsIDs = new int[1] { 100 };
    private int[] currentContainersAndStationsIDs = new int[1] { 100 };

    [SerializeField]
    private int _000ChangeState2ID = -1, _100ChangeState2ID = -1;
    private int currentChangeState2ID = -1;
    [SerializeField]
    private bool isState2Changer = false;

    [SerializeField]
    public float[] cookingTimes = new float[3] {0, 0, 0};
    public float[] maxCookingTimes = new float[3] { 0, 0, 0 };
    // 0- stove, 1- cutting board, 2- Deep Fryer

    private void Awake()
    {
        ChangeAttributesIDs();
    }

    public void ChangeState1(int newState, bool showModel)
    {
        ingredientState1 = newState;
        HideModel();
        UpdateModel();
        if (showModel)
            ShowModel();
        ChangeAttributesIDs();
    }

    public void ChangeState2(int newState, bool showModel)
    {
        ingredientState2 = newState;
        HideModel();
        UpdateModel();
        if (showModel)
            ShowModel();
        ChangeAttributesIDs();
    }

    public void ChangeState3(int newState, bool showModel)
    {
        ingredientState3 = newState;
        HideModel();
        UpdateModel();
        if (showModel)
            ShowModel();
        ChangeAttributesIDs();
    }

    private void ChangeAttributesIDs()
    {
        if (ingredientState1 == 0 && ingredientState2 == 0 && ingredientState3 == 0)
        {
            currentMixIDs = _000MixIDs;
            currentContainersAndStationsIDs = _000ContainersAndStationsIDs;
            currentChangeState2ID = _000ChangeState2ID;
        }
        else if (ingredientState1 == 0 && ingredientState2 == 0 && ingredientState3 == 1)
        {
            currentMixIDs = _001MixIDs;
            currentContainersAndStationsIDs = _001ContainersAndStationsIDs;
            currentChangeState2ID = -1;
        }
        else if (ingredientState1 == 0 && ingredientState2 == 1 && ingredientState3 == 0)
        {
            currentMixIDs = new int[0];
            currentContainersAndStationsIDs = _010ContainersAndStationsIDs;
            currentChangeState2ID = -1;
        }
        else if (ingredientState1 == 0 && ingredientState2 == 1 && ingredientState3 == 2)
        {
            currentMixIDs = _012MixIDs;
            currentContainersAndStationsIDs = new int[1] { 100 };
            currentChangeState2ID = -1;
        }
        else if (ingredientState1 == 1 && ingredientState2 == 0 && ingredientState3 == 0)
        {
            currentMixIDs = _100MixIDs;
            currentContainersAndStationsIDs = _100ContainersAndStationsIDs;
            currentChangeState2ID = _100ChangeState2ID;
        }
        else if (ingredientState1 == 1 && ingredientState2 == 0 && ingredientState3 == 2)
        {
            currentMixIDs = _102MixIDs;
            currentContainersAndStationsIDs = new int[1] { 100 };
            currentChangeState2ID = -1;
        }
        else if (ingredientState1 == 1 && ingredientState2 == 1 && ingredientState3 == 0)
        {
            currentMixIDs = new int[0];
            currentContainersAndStationsIDs = _110ContainersAndStationsIDs;
            currentChangeState2ID = -1;
        }
        else if (ingredientState1 == 1 && ingredientState2 == 1 && ingredientState3 == 2)
        {
            currentMixIDs = _112MixIDs;
            currentContainersAndStationsIDs = new int[1] { 100 };
            currentChangeState2ID = -1;
        }
        else
        {
            currentMixIDs = new int[0];
            if(ingredientState3 != -1)
            {
                currentContainersAndStationsIDs = new int[1] { 100 };
            }
            else
            {
                //si esta quemado, no puede entrar a ningun recipiente mas que la basura
                currentContainersAndStationsIDs = new int[1] { 502 };
            }
            currentChangeState2ID = -1;
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

    public int[] ShowContainersAndStationsIDs()
    {
        return currentContainersAndStationsIDs;
    }

    public int ShowChangeState2ID()
    {
        return currentChangeState2ID;
    }

    public bool ShowIfIsState2Changer()
    {
        return isState2Changer;
    }

    public void ShowModel()
    {
        lastModel.SetActive(true);
    }

    private void UpdateModel()
    {
        if(ingredientState3 != -1)
        {
            switch (ingredientState1)
            {
                case 0:
                    switch (ingredientState2)
                    {
                        case 0:
                            lastModel = ingredientModels00[ingredientState3];
                            break;
                        case 1:
                            lastModel = ingredientModels01[ingredientState3];
                            break;
                    }
                    break;
                case 1:
                    switch (ingredientState2)
                    {
                        case 0:
                            lastModel = ingredientModels10[ingredientState3];
                            break;
                        case 1:
                            lastModel = ingredientModels11[ingredientState3];
                            break;
                    }
                    break;
            }
        }
    }

    public void HideModel()
    {
        lastModel.SetActive(false);
    }
}
