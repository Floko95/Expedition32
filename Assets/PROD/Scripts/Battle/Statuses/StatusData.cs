using UnityEngine;

[CreateAssetMenu(fileName = "status", menuName = "Ex32/StatusData", order = 0)]

public class StatusData : ScriptableObject{
    
    [Header("Display")]
    public bool isDebuff;
    public float Value;
    public int Duration;
    public bool isStackable;
    
    
    [Header("Display")]
    public string title;
    public Sprite icon;
    
    [TextArea(3, 5)]
    public string desc;
}
