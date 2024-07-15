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
    private GameObject lastModel; //quiza haga falta un void start donde se iguale lastModel al primer modelo

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
    private int[] _000ContainerIDs, _001ContainerIDs, _010ContainerIDs, _100ContainerIDs, _110ContainerIDs;
    private int[] currentContainerIDs = new int[1] { 0 };

    [SerializeField]
    private int _000ChangeState2ID, _100ChangeState2ID;
    private int currentChangeState2ID = 0;
    [SerializeField]
    private bool isState2Changer;

    //Esto servira para mostrar la barra de que tan completado esta el proceso
    [SerializeField]
    private int state1To1Time = 6, state3To1Time = 40, state3To2Time = 40, burnTime = 10;
    private int currentProcessTime = 0;
    private int currentProcessPercent = 0;

    public void ChangeState1(int newState, bool showModel)
    {
        ingredientState1 = newState;
        HideModel();
        UpdateModel();
        if (showModel)
            ShowModel();
        ChangeAttributesIDs();
        //Por ahora, cada que cambia de estado se resetea el processPercent, por si las dudas
        ResetProcessPercent();
    }

    public void ChangeState2(int newState, bool showModel)
    {
        ingredientState2 = newState;
        HideModel();
        UpdateModel();
        if (showModel)
            ShowModel();
        ChangeAttributesIDs();
        ResetProcessPercent();
    }

    public void ChangeState3(int newState, bool showModel)
    {
        ingredientState3 = newState;
        HideModel();
        UpdateModel();
        if (showModel)
            ShowModel();
        ChangeAttributesIDs();
        ResetProcessPercent();
    }

    private void ChangeAttributesIDs()
    {
        if (ingredientState1 == 0 && ingredientState2 == 0 && ingredientState3 == 0)
        {
            currentMixIDs = _000MixIDs;
            currentContainerIDs = _000ContainerIDs;
            currentChangeState2ID = _000ChangeState2ID;
        }
        else if (ingredientState1 == 0 && ingredientState2 == 0 && ingredientState3 == 1)
        {
            currentMixIDs = _001MixIDs;
            currentContainerIDs = _001ContainerIDs;
            currentChangeState2ID = 0;
        }
        else if (ingredientState1 == 0 && ingredientState2 == 1 && ingredientState3 == 0)
        {
            currentMixIDs = new int[0];
            currentContainerIDs = _010ContainerIDs;
            currentChangeState2ID = 0;
        }
        else if (ingredientState1 == 0 && ingredientState2 == 1 && ingredientState3 == 2)
        {
            currentMixIDs = _012MixIDs;
            currentContainerIDs = new int[1] { 0 };
            currentChangeState2ID = 0;
        }
        else if (ingredientState1 == 1 && ingredientState2 == 0 && ingredientState3 == 0)
        {
            currentMixIDs = _100MixIDs;
            currentContainerIDs = _100ContainerIDs;
            currentChangeState2ID = _100ChangeState2ID;
        }
        else if (ingredientState1 == 1 && ingredientState2 == 0 && ingredientState3 == 2)
        {
            currentMixIDs = _102MixIDs;
            currentContainerIDs = new int[1] { 0 };
            currentChangeState2ID = 0;
        }
        else if (ingredientState1 == 1 && ingredientState2 == 1 && ingredientState3 == 0)
        {
            currentMixIDs = new int[0];
            currentContainerIDs = _110ContainerIDs;
            currentChangeState2ID = 0;
        }
        else if (ingredientState1 == 1 && ingredientState2 == 1 && ingredientState3 == 2)
        {
            currentMixIDs = _112MixIDs;
            currentContainerIDs = new int[1] { 0 };
            currentChangeState2ID = 0;
        }
        else
        {
            currentMixIDs = new int[0];
            currentContainerIDs = new int[1] { 0 };
            currentChangeState2ID = 0;
        }
    }

    public void ChangeProcessTime(int stateNumber, int stateValue)
    {
        switch (stateNumber)
        {
            case 1:
                switch (stateValue)
                {
                    case 1:
                        //cortar
                        currentProcessTime = state1To1Time;
                        break;
                }
                break;
            case 2:
                //esto probablemente nunca se use, pero aca esta pos nomas
                break;
            case 3:
                switch (stateValue)
                {
                    case 1:
                        //cocinar en olla
                        currentProcessTime = state3To1Time;
                        break;
                    case 2:
                        //freir
                        currentProcessTime = state3To2Time;
                        break;
                }
                break;
            case 666:
                //aca es cuando sea pa quemarse
                currentProcessTime = burnTime;
                break;
        }
    }

    public void ChangeProcessPercent()
    {
        currentProcessPercent += 1;
    }

    public bool IsReady()
    {
        if (currentProcessTime != 0 && currentProcessPercent >= currentProcessTime)
            return true;
        return false;
    }

    public void ResetProcessPercent()
    {
        currentProcessPercent = 0;
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

    public int[] ShowContainerIDs()
    {
        return currentContainerIDs;
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

    public void HideModel()
    {
        lastModel.SetActive(false);
    }
}
