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
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            FlipCharacter(true);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            FlipCharacter(false);
        }
    }

    private void FlipCharacter(bool flipX)
    {
        _spriteRenderer.flipX = flipX;
        
        }
    }