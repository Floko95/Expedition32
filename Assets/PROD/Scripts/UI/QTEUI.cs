using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class QTEUI : MonoBehaviour, IInitializable<QTE>
{
    [SerializeField] private Image fillImageQTE;
    
    [Title("Feedbacks")]
    [SerializeField] private MMF_Player completeFeedback;
    [SerializeField] private MMF_Player stoppedFeedback;
    
    public QTE qte { get; private set; }
    
    public void Init(QTE data) {
        qte = data;
    }

    public void UpdateCountdown(float timeTillEnd) {
        fillImageQTE.fillAmount = timeTillEnd;
    }
    
    public void Stop(bool success) {
        if (success) completeFeedback.PlayFeedbacks();
        else stoppedFeedback.PlayFeedbacks();
        
        fillImageQTE.fillAmount = 0f;
        
        Destroy(gameObject, success ? completeFeedback.TotalDuration : stoppedFeedback.TotalDuration);
    }
    
    bool IInitializable<QTE>.Initialized { get; set; } = false;
}
