using UnityEngine;

public class FlipSprite : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            FlipCharacter(true);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            FlipCharacter(false);
        }
    }

    private void FlipCharacter(bool flipX)
    {
        _spriteRenderer.flipX = flipX;
    }
}
