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
        var dn = damageDNPrefab.Spawn(target.transform.position +APDNOffset, amount);
        dn.numberSettings.customColor = isCrit;

        dn.enableBottomText = reaction is not ElementReaction.Normal;
        dn.enableRightText = damageType is not ElementType.Physical;
        
        if(damageType is not ElementType.Physical)
            dn.rightText = "<sprite name=\"" + damageType + "\">";
        
        switch (reaction) {
            case ElementReaction.Weak:
                dn.bottomText = "Weak";
                break;
            case ElementReaction.Resistant:
                dn.bottomText = "Resistant";
                break;
            case ElementReaction.Immune:
                dn.bottomText = "Immune";
                break;
            case ElementReaction.Absorb:
                dn.bottomText = "Absorbed!";
                break;
            default:
                break;
        }
    }
}
