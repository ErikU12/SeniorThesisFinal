using UnityEngine;

namespace Player
{
    public class PlayerMelee : MonoBehaviour
    {
        public float immunityDuration = 5f; // Duration of immunity effect in seconds
        public float meleeCooldown = 10f; // Cooldown duration for melee attack in seconds
        public AnimationClip immuneAnimation; // The animation clip to play when immune
        private Animator _animator; // Reference to the Animator component
        private bool _isImmune;
        private PlayerHealth _playerHealth;
        private float _immunityTimer;
        private float _cooldownTimer;

        // Properties to check if the player is immune or if the melee attack is on cooldown
        public bool IsImmune { get { return _isImmune; } }
        public bool IsCooldownActive { get { return _cooldownTimer > 0; } }

        private void Start()
        {
            _animator = GetComponent<Animator>(); // Get reference to Animator component
            _playerHealth = GetComponent<PlayerHealth>(); // Get reference to PlayerHealth script
            _immunityTimer = 0f; // Initialize immunity timer
            _cooldownTimer = 0f; // Initialize cooldown timer
        }

        private void Update()
        {
            // Update immunity timer
            if (_immunityTimer > 0)
            {
                _immunityTimer -= Time.deltaTime;
            }

            // Update cooldown timer
            if (_cooldownTimer > 0)
            {
                _cooldownTimer -= Time.deltaTime;
            }

            // Check for player input to activate melee attack
            if (Input.GetKeyDown(KeyCode.C) && _cooldownTimer <= 0)
            {
                ActivateDamageEffect();
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            // Check if the player is immune and collides with an enemy
            if (_isImmune && collision.gameObject.CompareTag("Enemy"))
            {
                // Increment player's health upon collision with an enemy
                _playerHealth.IncreaseHealth(1);
                // Deal damage to the enemy
                Destroy(collision.gameObject);
            }

            // Check if the collision is with a trigger collider (isTrigger)
            if (_isImmune && collision.collider.isTrigger)
            {
                // Destroy the GameObject if it's a trigger collider
                Destroy(collision.gameObject);
            }
        }

        private void ActivateDamageEffect()
        {
            _isImmune = true;
            _animator.enabled = true; // Enable Animator component
            _animator.Play(immuneAnimation.name); // Play the immune animation
            Invoke("DeactivateDamageEffect", immunityDuration);
            _immunityTimer = immunityDuration; // Start immunity timer
            _cooldownTimer = meleeCooldown; // Start cooldown timer
        }

        private void DeactivateDamageEffect()
        {
            _isImmune = false;
            _animator.enabled = true; // Re-enable Animator component
        }
    }
} 
