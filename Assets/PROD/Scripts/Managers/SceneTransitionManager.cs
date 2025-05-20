using System.Collections;
using System.Threading.Tasks;
using Eflatun.SceneReference;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour {

    [SerializeField] private Canvas transitionCanvas;
    [SerializeField] private MMF_Player exitTransitionMMF;
    [SerializeField] private MMF_Player enterTransitionMMF;

    public bool IsLoading => _loadingOperation != null || _unloadOriginOperation != null;
    
    private AsyncOperation _loadingOperation;
    private AsyncOperation _unloadOriginOperation;
    
    private UnityAction _loadedSceneCallback;
    
    private async void Awake() {
        await Toolbox.WaitUntilReadyAsync();
        Toolbox.Set(this);
        
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
        transform.SetParent(null);
        
        transitionCanvas.enabled = true;
    }

    public void LoadScene(SceneReference sceneReference, UnityAction onSceneLoaded = null) {
        if (IsLoading) return;
        
        _loadedSceneCallback = onSceneLoaded;
        StartCoroutine(LoadSequence(sceneReference));
    }

    protected virtual IEnumerator LoadSequence(SceneReference sceneReference) {
        yield return FadeIn();
        yield return Unload();
        yield return Load(sceneReference);
        yield return FadeOut();
    }

    protected virtual IEnumerator FadeIn() {
        yield return exitTransitionMMF.PlayFeedbacksCoroutine(transform.position);        
    }

    protected virtual IEnumerator FadeOut() {
        yield return enterTransitionMMF.PlayFeedbacksCoroutine(transform.position);
    }

    protected virtual IEnumerator Load(SceneReference sceneReference) {
        _loadingOperation = SceneManager.LoadSceneAsync(sceneReference.Name, LoadSceneMode.Additive);
        
        while (!_loadingOperation.isDone) {
            yield return null;
        }
        
        _loadingOperation = null;
    }
    
    protected virtual IEnumerator Unload() {
        _unloadOriginOperation = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        
        while (!_unloadOriginOperation.isDone) {
            yield return null;
        }

        _unloadOriginOperation = null;
    }
    
    private void OnActiveSceneChanged(Scene arg0, Scene arg1) {
        _loadedSceneCallback?.Invoke();
        _loadedSceneCallback = null;
    }
}
