using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;

public class BattleLogUI : MMSingleton<BattleLogUI>
{
    [SerializeField] private TextMeshProUGUI logText;

    [SerializeField] private MMF_Player logFeel;
    
    public static void Log(string text) => Instance.LogEntry(text);

    public void LogEntry(string text) {
        logText.text = text;
        logFeel?.PlayFeedbacks();
    }
}
