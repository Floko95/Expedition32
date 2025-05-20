using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-100)]
public class Bootstrapper : MonoBehaviour {
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init() {
        Debug.Log("Initialized Bootstrapper");
        if (SceneManager.GetSceneByName("Persistent").isLoaded) return;
        
        SceneManager.LoadSceneAsync("Persistent", LoadSceneMode.Additive);
    }
}