using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthContainer : MonoBehaviour
{
    [SerializeField] private Image fillAmountImage;
    [SerializeField] public TextMeshProUGUI healthText;
    public Image FillAmountImage => fillAmountImage;
    //public TextMeshProUGUI HealthText => healthText;
}
