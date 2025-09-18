using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Minigames/New Catalog")]
public class MinigameCatalogSO : ScriptableObject
{
    public List<MinigameSO> minigames;
}
