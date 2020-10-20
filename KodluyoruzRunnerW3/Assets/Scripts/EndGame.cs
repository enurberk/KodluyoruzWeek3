using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "EndPlatform")
        {
            SceneManager.LoadScene(2);
        }
    }
}
