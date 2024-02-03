using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public string sceneNameToLoad; // The name of the scene to load (set in the Inspector)

    private Button _button;

    private void Start()
    {
        // Get the Button component attached to this GameObject
        _button = GetComponent<Button>();

        // Add a listener to the button's onClick event
        _button.onClick.AddListener(LoadMainScene);
    }

    private void LoadMainScene()
    {
        // Load the specified scene
        SceneManager.LoadScene(sceneNameToLoad);
    }
}