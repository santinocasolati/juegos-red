using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct DisplayFrames
{
    public Sprite[] sprites;
}

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] private List<DisplayFrames> characterSprites;
    [SerializeField] private UISpriteAnimator characterImg;
    [SerializeField] private Button prevBtn;
    [SerializeField] private Button nextBtn;

    private int currentIndex = 0;

    public int CurrentIndex { get { return currentIndex; } }

    private void Awake()
    {
        SetCharacter(PlayerPrefs.GetInt("Character"));
    }

    private void OnEnable()
    {
        prevBtn.onClick.AddListener(() => SetCharacter(-1));
        nextBtn.onClick.AddListener(() => SetCharacter(1));
    }

    private void OnDisable()
    {
        prevBtn.onClick.RemoveAllListeners();
        nextBtn.onClick.RemoveAllListeners();
    }

    private void SetCharacter(int direction)
    {
        int updatedIndex = currentIndex + direction;

        if (updatedIndex >= characterSprites.Count)
            updatedIndex = 0;
        else if (updatedIndex < 0)
            updatedIndex = characterSprites.Count - 1;

        characterImg.frames = characterSprites[updatedIndex].sprites;

        currentIndex = updatedIndex;
    }
}
