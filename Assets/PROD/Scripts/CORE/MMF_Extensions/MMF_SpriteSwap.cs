using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;


[AddComponentMenu("")]
[FeedbackPath("UI/Sprite Swap")]
public class MMF_SpriteSwap : MMF_Feedback
{
    [MMFInspectorGroup("UI", true, 64)]
    [SerializeField] public Image image;
    [SerializeField] public Sprite appliedSprite;
    
    protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1) {
        image.sprite = appliedSprite;
    }
}
