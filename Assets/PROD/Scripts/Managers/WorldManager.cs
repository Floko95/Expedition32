using Eflatun.SceneReference;
using MoreMountains.Tools;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField] private SceneReference battleSceneAsset;
    
    private SceneTransitionManager _sceneTransitionManager;
    
    private async void Awake() {
        await Toolbox.WaitUntilReadyAsync();
        Toolbox.Set(this);
    }

    private async void Start() {
        await Toolbox.WaitUntilReadyAsync();
        _sceneTransitionManager = Toolbox.Get<SceneTransitionManager>();
    }

    public void StartBattle(Enemy enemy) {
        var data = enemy.battleData;
        
        if (data.battleSoundTtrack) {
            
            MMSoundManagerSoundPlayEvent.Trigger(
                data.battleSoundTtrack,
                MMSoundManager.MMSoundManagerTracks.Music,
                Vector3.zero,
                loop: true,
                volume: 0.5f,
                pitch: 1f,
                panStereo: 0f,
                spatialBlend: 0f,
                bypassEffects: false,
                priority: 128,
                fade: true,
                persistent: true,
                soloSingleTrack: true,
                fadeDuration: 0.5f
            );
        }
        _sceneTransitionManager.LoadScene(battleSceneAsset, () => {
        });
    }
}
