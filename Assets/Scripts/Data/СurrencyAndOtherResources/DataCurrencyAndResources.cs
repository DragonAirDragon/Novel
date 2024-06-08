using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "DataCurrencyAndResources", menuName = "DataCurrencyAndResources", order = 0)]
public class DataCurrencyAndResources : SerializedScriptableObject
{
    public int countCrystals;
    public int countEnergy;
}
