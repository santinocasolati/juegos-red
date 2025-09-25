using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpriteAnimator : MonoBehaviour
{
    public Image targetImage;
    public Sprite[] frames;
    public float framesPerSecond = 10f;
    public bool loop = true;

    private int currentFrame = 0;
    private float timer = 0f;

    void Update()
    {
        if (frames.Length == 0 || targetImage == null) return;

        timer += Time.deltaTime;
        if (timer >= 1f / framesPerSecond)
        {
            timer -= 1f / framesPerSecond;
            currentFrame++;
            if (currentFrame >= frames.Length)
            {
                if (loop)
                    currentFrame = 0;
                else
                {
                    currentFrame = frames.Length - 1;
                    enabled = false;
                }
            }

            targetImage.sprite = frames[currentFrame];
        }
    }
}
