using UnityEngine;

public class RICKROLL : MonoBehaviour
{
    private bool konamiCodeActivated = false;

    // Prefab to replace enemies
    public GameObject enemyPrefab;

    // Reference to the audio source
    public AudioSource audioSource;

    void Update()
    {
        if (!konamiCodeActivated && Input.GetKeyDown(KeyCode.R))
        {
            konamiCodeActivated = true;
            Debug.Log("Konami Code Activated!");
            ReplaceEnemies();
            PlayMusic();
        }
    }

    void ReplaceEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Instantiate(enemyPrefab, enemy.transform.position, enemy.transform.rotation);
            Destroy(enemy);
        }
    }

    void PlayMusic()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}