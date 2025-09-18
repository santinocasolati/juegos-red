using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Minigames/New Minigame")]
public class MinigameSO : ScriptableObject
{
    public string displayName;
    public Sprite icon;
    public string scene;
}
