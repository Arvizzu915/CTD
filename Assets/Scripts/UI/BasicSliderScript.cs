using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicSliderScript : MonoBehaviour
{
    [SerializeField]
    private Slider slider, slider2;
    [SerializeField]
    private Image sliderFill, sliderState;
    [SerializeField]
    private Camera camera;

    private void Update()
    {
        if (camera != null)
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, camera.transform.rotation, 50f * Time.deltaTime);
    }

    public void ShowSlider()
    {
        //slider.enabled = true;
        slider.gameObject.SetActive(true);
    }

    public void HideSlider()
    {
        //slider.enabled = false;
        slider.gameObject.SetActive(false);
    }

    public void SetValues(float startingValue, float minValue, float maxValue)
    {
        slider.minValue = minValue;
        slider.maxValue = maxValue;
        slider.value = startingValue;
    }

    public void ChangeValue(float ammount)
    {
        slider.value += ammount;
    }

    public void ChangeFillColor(int processState)
    {
        // 2 verde, 1 rojo
        if (sliderFill == null)
            return;
        switch (processState)
        {
            case 1:
                sliderFill.color = Color.red;
                break;
            case 2:
                sliderFill.color = Color.green;
                break;
        }
    }

    public void ChangeStateColor(int processState)
    {
        // -1 nada, 0 rojo, 1 verde
        if (sliderState == null)
            return;
        switch (processState)
        {
            case -1:
                sliderState.color = Color.gray;
                break;
            case 0:
                sliderState.color = Color.red;
                break;
            case 1:
                sliderState.color = Color.green;
                break;
        }
    }

    public void ShowSlider2()
    {
        slider2.gameObject.SetActive(true);
    }

    public void HideSlider2()
    {
        slider2.gameObject.SetActive(false);
    }

    public void SetValues2(float startingValue, float minValue, float maxValue)
    {
        slider2.minValue = minValue;
        slider2.maxValue = maxValue;
        slider2.value = startingValue;
    }

    public void ChangeValue2(float ammount)
    {
        slider2.value += ammount;
    }
}
