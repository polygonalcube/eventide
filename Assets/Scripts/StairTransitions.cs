using UnityEngine;
using UnityEngine.SceneManagement;

public class StairTransitions : MonoBehaviour
{
    // 

    // 

    [SerializeField] bool travelUp = true;
    [SerializeField] string customScene = "";
    
    void OnTriggerEnter2D(Collider2D col)
    {
        LayerMask playerLayer = LayerMask.NameToLayer("Player");
        if (col.gameObject.layer == playerLayer)
        {
            if (customScene.Length > 0 && SceneManager.GetSceneByName(customScene).IsValid()) SceneManager.LoadScene(customScene);
            else if (travelUp) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }
}
