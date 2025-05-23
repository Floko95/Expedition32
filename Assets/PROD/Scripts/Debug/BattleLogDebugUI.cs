using DG.Tweening;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class BattleLogDebugUI : MMSingleton<BattleLogDebugUI>
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform content;
    [SerializeField, AssetsOnly] private BattleLogEntry battleLogEntryPrefab;
    
    [SerializeField] private int maxEntriesDisplayed;
    
    private int nbEntries;

    public static void Log(string text) => Instance.LogEntry(text);

    public void LogEntry(string text) {
        var entry = Instantiate(battleLogEntryPrefab, content);
        entry.SetText(text);
        
        nbEntries++;
        if (nbEntries > maxEntriesDisplayed) {
            Destroy(content.GetChild(0).gameObject);
        }

        DOTween.To((x) => scrollRect.verticalNormalizedPosition = x, scrollRect.verticalNormalizedPosition, 0, 0.5f);
    }
}
