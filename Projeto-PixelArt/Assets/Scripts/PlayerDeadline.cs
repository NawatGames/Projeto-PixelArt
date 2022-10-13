using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeadline : MonoBehaviour
{
    [SerializeField] private string playerTag;
    private int currentSceneIndex;

    private void Start()
    {
        playerTag ??= "Player";
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == playerTag)
            SceneManager.LoadScene(currentSceneIndex);
    }
}
