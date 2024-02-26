using UnityEngine;

namespace Player
{
    public class PlayerMelee : MonoBehaviour
    {
        public float immunityDuration = 5f; // Duration of immunity effect in seconds
        public Sprite immuneSprite; // The sprite to use when immune
        private SpriteRenderer _spriteRenderer;
        private bool _isImmune;
        private PlayerHealth _playerHealth;
        private Sprite _regularSprite; // Reference to the regular sprite
        private Animator _animator; // Reference to the Animator component

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _playerHealth = GetComponent<PlayerHealth>(); // Get reference to PlayerHealth script
            _regularSprite = _spriteRenderer.sprite; // Store reference to the regular sprite
            _animator = GetComponent<Animator>(); // Get reference to Animator component
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                ActivateDamageEffect();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_isImmune && collision.gameObject.CompareTag("Enemy"))
            {
                // Increment player's health upon collision with an enemy
                _playerHealth.IncreaseHealth(1);
                // Deal damage to the enemy
                Destroy(collision.gameObject);
            }
        }

        private void ActivateDamageEffect()
        {
            _isImmune = true;
            _animator.enabled = false; // Disable Animator component
            _spriteRenderer.sprite = immuneSprite;
            Invoke("DeactivateDamageEffect", immunityDuration);
        }

        private void DeactivateDamageEffect()
        {
            _isImmune = false;
            _animator.enabled = true; // Re-enable Animator component
            // Change sprite back to regular sprite
            _spriteRenderer.sprite = _regularSprite;
        }
    }
}
