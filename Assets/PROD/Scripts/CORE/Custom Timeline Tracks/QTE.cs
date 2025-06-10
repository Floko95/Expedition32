using System;
using BitDuc.EnhancedTimeline.Observable;
using OSLib.StatSystem;
using UnityEngine;

[Serializable]
public class QTE : SimpleClip {
    [SerializeField] public bool isComplete = false;
    
    [SerializeField] public Vector2 screenAnchorPos;
    [SerializeField] public StatModifier appliedModifierOnSuccess;
}
