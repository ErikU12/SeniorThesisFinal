using System.Collections;
using UnityEngine;

public class FISHMODE : MonoBehaviour
{
    private static readonly KeyCode[] KonamiSequence =
    {
        KeyCode.UpArrow, KeyCode.UpArrow,
        KeyCode.DownArrow, KeyCode.DownArrow,
        KeyCode.LeftArrow, KeyCode.RightArrow,
        KeyCode.LeftArrow, KeyCode.RightArrow,
        KeyCode.B, KeyCode.A
    };

    private int currentInputIndex = 0;
    private bool konamiCodeActivated = false;

    // Reference to your player character
    public GameObject playerCharacter;
    private Rigidbody2D playerRigidbody;

    void Start()
    {
        playerRigidbody = playerCharacter.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!konamiCodeActivated)
        {
            CheckKonamiCodeInput();
        }
        else
        {
            // Perform actions when Konami Code is activated
            playerRigidbody.constraints = RigidbodyConstraints2D.None; // Unrestrict all constraints
            playerCharacter.transform.rotation = Quaternion.Euler(0f, 0f, 90f); // Rotate 90 degrees
            StartCoroutine(RainbowColor());
        }
    }

    void CheckKonamiCodeInput()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KonamiSequence[currentInputIndex]))
            {
                currentInputIndex++;
                if (currentInputIndex == KonamiSequence.Length)
                {
                    // Konami Code activated
                    konamiCodeActivated = true;
                    Debug.Log("Konami Code Activated!");
                }
            }
            else
            {
                currentInputIndex = 0;
            }
        }
    }

    IEnumerator RainbowColor()
    {
        while (true)
        {
            float hue = Mathf.PingPong(Time.time * 0.5f, 1f);
            Color color = Color.HSVToRGB(hue, 1f, 1f);
            playerCharacter.GetComponent<Renderer>().material.color = color;
            yield return null;
        }
    }
}
