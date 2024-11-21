using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FactItem : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI factName;
    [SerializeField] private TMPro.TextMeshProUGUI factId;

    public TextMeshProUGUI FactName { get => factName; }
    public TextMeshProUGUI FactId { get => factId; }
}
