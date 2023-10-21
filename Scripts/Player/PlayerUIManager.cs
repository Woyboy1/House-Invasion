using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerUIManager : NetworkBehaviour
{
    [Header("Host Player References")]
    [SerializeField] private GameObject hostDisplay;
    [SerializeField] private TextMeshProUGUI joinCodeText;

    [Header("Network References")]
    [SerializeField] private TextMeshProUGUI regionText;
    public void UpdateJoinCode(string joinCode)
    {
        joinCodeText.text = joinCode;
    }

    public void UpdateRegionText(string regionText)
    {
        this.regionText.text = "Region: " + regionText;
    }

    public void EnableDisplayHostUI()
    {
        hostDisplay.SetActive(true);
    }

    public void DisableDisplayHostUI()
    {
        hostDisplay.SetActive(false);
    }
}
