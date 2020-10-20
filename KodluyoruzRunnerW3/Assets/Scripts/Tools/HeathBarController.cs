using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//bu scrpitin çalışması için slider componentine ihtiyaç var demek;
[RequireComponent(typeof(Slider))]
public class HeathBarController : MonoBehaviour
{
    private Slider _slider;
    [SerializeField] private TextMeshProUGUI healthText;
    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    public void UpdateSliderValue(int hp)
    {
        healthText.text = hp.ToString();
        _slider.value = (float)hp / 100;
    }

}
