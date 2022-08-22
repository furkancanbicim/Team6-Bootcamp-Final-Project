using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplayScript : MonoBehaviour
{
    [Header("Card Data")]
    [SerializeField] private CardScriptableObjects cardDisplay;

    [Header("Cart Properties")]
    [SerializeField] private Text nameText;
    [SerializeField] private Text healthText;
    [SerializeField] private Text attackText;

    private void Start()
    {
        nameText.text = cardDisplay.name;
        healthText.text = cardDisplay.health.ToString();
        attackText.text = cardDisplay.attack.ToString();
    }
}
