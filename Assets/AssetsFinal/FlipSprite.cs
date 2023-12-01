using UnityEngine;

namespace Scenes.Scripts
{
    public class FlipSprite : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        public Transform bulletSpawnPoint; // Assign the bullet spawn point in the inspector

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

            // Flip the bullet spawn point along with the character
            if (bulletSpawnPoint != null)
            {
                Vector3 scale = bulletSpawnPoint.localScale;
                scale.x *= -1;
                bulletSpawnPoint.localScale = scale;
            }
        }
    }
}