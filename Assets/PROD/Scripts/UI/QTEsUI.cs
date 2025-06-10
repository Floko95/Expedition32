using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class QTEsUI : MonoBehaviour
{
    [SerializeField] private QTEUI qtePrefab;
    [SerializeField] private RectTransform qteSpawnRect;
    
    private Image _qteImage;
    private List<QTEUI> _qteUIs = new List<QTEUI>();
    
    public void StartQTE(QTE qte) {
        var qteUI = Instantiate(qtePrefab, qteSpawnRect);
        var rectTransform = qteUI.transform as RectTransform;
        rectTransform.anchoredPosition = qte.screenAnchorPos;
            
        qteUI.Init(qte);
        
        _qteUIs.Add(qteUI);
    }
    
    public void UpdateQTECountdown(QTE qte, float timeTillEnd) {
        var qteUi = _qteUIs.FirstOrDefault(ui => ui.qte == qte);
        if (qteUi == null) return;
        
        qteUi.UpdateCountdown(timeTillEnd);
    }

    public void StopQTE(QTE qte, bool success) {
        var qteUi = _qteUIs.FirstOrDefault(ui => ui.qte == qte);
        if (qteUi == null) return;
        
        qteUi.Stop(success);
        
        _qteUIs.Remove(qteUi);
    }
}
