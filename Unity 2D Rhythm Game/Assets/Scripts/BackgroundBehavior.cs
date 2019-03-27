using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundBehavior : MonoBehaviour
{
    // TODO : https://educast.com/course/game/NZ46/lecture/37

    public GameObject gameBackround;
    private SpriteRenderer gameBackroundSpriteRenderer;

    private void Start()
    {
        gameBackroundSpriteRenderer = gameBackround.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeOut(gameBackroundSpriteRenderer));
    }

    private IEnumerator FadeOut (
        SpriteRenderer spriteRenderer
        , float amount = 0.005f
        , float timeGap = 0.005f )
    {
        Color color = spriteRenderer.color;

        while(color.a > 0.0f)
        {
            color.a -= amount;
            spriteRenderer.color = color;

            yield return new WaitForSeconds(timeGap);
        }
    }

    private void Update()
    {
        
    }
}
