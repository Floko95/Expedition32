using System;
using DamageNumbersPro;
using UnityEngine;

public class DNManager : MonoBehaviour
{
    [SerializeField] private DamageNumber damageDNPrefab;
    [SerializeField] private DamageNumber healDNPrefab;
    
    private async void Start() {
        await Toolbox.WaitUntilReadyAsync();
        BattleLogic.DamagingEvent += OnDamagingEvent;
        BattleLogic.HealingEvent += OnHealingEvent;
    }

    private void OnHealingEvent(Unit source, Unit target, float amount) {
        throw new NotImplementedException();
    }

    private void OnDestroy() {
        BattleLogic.DamagingEvent -= OnDamagingEvent;
        BattleLogic.HealingEvent -= OnHealingEvent;
    }

    private void OnDamagingEvent(Unit source, Unit target, float amount, bool isCrit) {
        damageDNPrefab.Spawn(target.transform.position, amount);
        damageDNPrefab.enableColorByNumber = isCrit;
    }
}
