using UnityEngine;

namespace Scenes.Scripts
{
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
                _spriteRenderer.flipX = true; 
            }

           
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                _spriteRenderer.flipX = false; 
            }
        }
    }
}