using UnityEngine;
using UnityEngine.UI;

public class TurnUI : MonoBehaviour
{
    [SerializeField] private Image portraitImage;
    
    public void Init(UnitData unitData) {
        portraitImage.sprite = unitData.portrait;
    }
}
