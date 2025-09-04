using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] private List<Sprite> characterSprites;
    [SerializeField] private Image characterImg;
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

        characterImg.sprite = characterSprites[updatedIndex];

        currentIndex = updatedIndex;
    }
}
