using MoreMountains.Tools;
using Sirenix.OdinInspector;
using UnityEngine;

public class BattleLogDebug : MMSingleton<BattleLogDebug>
{
    [SerializeField] private RectTransform content;
    [SerializeField, AssetsOnly] private BattleLogEntry battleLogEntryPrefab;

    public static void Log(string text) {
        var entry = Instantiate(Instance.battleLogEntryPrefab, Instance.content);
        entry.SetText(text);
    }
}
