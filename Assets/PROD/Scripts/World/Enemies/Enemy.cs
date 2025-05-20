using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField] public BattleData battleData;
    
    private WorldManager _worldManager;
    
    private void Start() {
        _worldManager = Toolbox.Get<WorldManager>();
    }

    public void StartBattle() {
        _worldManager.StartBattle(this);
    }
    
    //TODO handle respawn
}
