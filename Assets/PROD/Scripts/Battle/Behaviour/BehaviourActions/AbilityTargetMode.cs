using Unity.Behavior;

[BlackboardEnum]
public enum AbilityTargetMode
{
    None = 0,
    
    CharacterSelf = 4,
    Ally = 6,
    AllAllies = 7,
    DeadAllies = 8,
    
    SelectTarget = 30,
    AllEnemies = 31,
}
