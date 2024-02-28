using UnityEngine;

namespace Player
{
    public class PlayerMelee : MonoBehaviour
    {
        public float immunityDuration = 5f; // Duration of immunity effect in seconds
        public AnimationClip immuneAnimation; // The animation clip to play when immune
        private Animator _animator; // Reference to the Animator component
        private bool _isImmune;
        private PlayerHealth _playerHealth;

        private void Start()
        {
            _animator = GetComponent<Animator>(); // Get reference to Animator component
            _playerHealth = GetComponent<PlayerHealth>(); // Get reference to PlayerHealth script
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
            _animator.enabled = true; // Enable Animator component
            _animator.Play(immuneAnimation.name); // Play the immune animation
            Invoke("DeactivateDamageEffect", immunityDuration);
        }

        private void DeactivateDamageEffect()
        {
            _isImmune = false;
            _animator.enabled = true; // Re-enable Animator component
        }
    }
}

