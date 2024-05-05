using UnityEngine;

namespace Player
{
    public class MeleeUI : MonoBehaviour
    {
        [SerializeField] private float cooldownDuration = 30f; // Cooldown duration in seconds
        [SerializeField] private Color grayColor = new Color(0.3f, 0.3f, 0.3f); // Darker gray color

        private SpriteRenderer _spriteRenderer;
        private Color _originalColor;
        private bool _isGrayedOut = false;
        private bool _isCooldownActive = false;
        private float _cooldownTimer = 0f; // Timer for cooldown duration

        void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _originalColor = _spriteRenderer.color; // Store the original color
        }

        void Update()
        {
            if (_isCooldownActive)
            {
                // Update cooldown timer
                _cooldownTimer -= Time.deltaTime;

                if (_cooldownTimer <= 0)
                {
                    // Cooldown has ended, revert back to original color
                    SetGrayScale(false);
                    _isCooldownActive = false;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.C))
                {
                    // Start cooldown and change color to gray
                    SetGrayScale(true);
                    _isCooldownActive = true;
                    _cooldownTimer = cooldownDuration;
                }
            }
        }

        void SetGrayScale(bool grayedOut)
        {
            if (grayedOut)
            {
                // Set the sprite color to the darker gray color
                _spriteRenderer.color = grayColor;
                _isGrayedOut = true;
            }
            else
            {
                // Restore the original color
                _spriteRenderer.color = _originalColor;
                _isGrayedOut = false;
            }
        }
    }
}