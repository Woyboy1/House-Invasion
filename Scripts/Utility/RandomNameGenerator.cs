using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomNameGenerator : MonoBehaviour
{
    public string[] names;
    public TextMeshProUGUI nameText;

    void Start()
    {
        int randNum = Random.Range(0, names.Length);

        nameText.text = names[randNum];
    }
}
