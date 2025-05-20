using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour {

    [SerializeField] public List<UnitData> playableCharacters;
    
    private void Awake() {
        Toolbox.Set(this);
    }
}
