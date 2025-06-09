using DamageNumbersPro;
using UnityEngine;

public class DNManager : MonoBehaviour
{
    [SerializeField] private DamageNumber damageDNPrefab;
    [SerializeField] private DamageNumber healDNPrefab;
    [SerializeField] private DamageNumber APDNPrefab;
    [SerializeField] private Vector3 APDNOffset;
    
    private async void Start() {
        await Toolbox.WaitUntilReadyAsync();
        BattleLogic.DamagingEvent += OnDamagingEvent;
        BattleLogic.HealingEvent += OnHealingEvent;
        Unit.OnAnyUnitRegainedAP += OnAnyUnitRegainedAP;
    }

    private void OnAnyUnitRegainedAP(Unit unit, int amount) {
        APDNPrefab.Spawn(unit.transform.position + APDNOffset, amount);
    }

    private void OnHealingEvent(Unit source, Unit target, float amount) {
        healDNPrefab.Spawn(target.transform.position, amount);
    }

    private void OnDestroy() {
        BattleLogic.DamagingEvent -= OnDamagingEvent;
        BattleLogic.HealingEvent -= OnHealingEvent;
        Unit.OnAnyUnitRegainedAP -= OnAnyUnitRegainedAP;
    }

    private void OnDamagingEvent(Unit source, Unit target, float amount, bool isCrit, ElementType damageType, ElementReaction reaction) {
        damageDNPrefab.Spawn(target.transform.position, amount);
        damageDNPrefab.numberSettings.customColor = isCrit;

        damageDNPrefab.enableBottomText = true;
        damageDNPrefab.enableRightText = true;
        damageDNPrefab.rightText = "<sprite name=\"" + damageType + "\">";
        
        switch (reaction) {
            case ElementReaction.Weak:
                damageDNPrefab.bottomText = "Weak";
                break;
            case ElementReaction.Resistant:
                damageDNPrefab.bottomText = "Resistant";
                break;
            case ElementReaction.Immune:
                damageDNPrefab.bottomText = "Immune";
                break;
            case ElementReaction.Absorb:
                damageDNPrefab.bottomText = "Absorbed!";
                break;
            default:
                break;
        }
    }
}
