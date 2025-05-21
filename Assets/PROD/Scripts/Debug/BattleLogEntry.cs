using TMPro;
using UnityEngine;

public class BattleLogEntry : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    
    public void SetText(string text) {
        textMeshPro.text = text;
    }
}
