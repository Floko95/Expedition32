using Eflatun.SceneReference;
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

        _sceneTransitionManager.LoadScene(battleSceneAsset,
            () => {
                /*var battleManager = Toolbox.Get<BattleManager>();
                battleManager.BattleData = data;*/
            });
    }
}
