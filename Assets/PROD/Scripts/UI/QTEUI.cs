using UnityEngine;
using UnityEngine.UI;

public class QTEUI : MonoBehaviour
{
    [SerializeField] private GameObject qtePrefab;
    [SerializeField] private RectTransform qteSpawnRect;
    
    private GameObject _qte;
    private Image _qteImage;
    
    public void StartQTE() {
        _qte = Instantiate(qtePrefab, qteSpawnRect);
        _qteImage = _qte.GetComponentInChildren<Image>();
    }
    
    public void UpdateQTECountdown(float timeTillEnd) {
        _qteImage.fillAmount = timeTillEnd;
    }

    public void StopQTE() {
        Destroy(_qte);
    }
}
