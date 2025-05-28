using TMPro;
using UnityEngine;

public class ItemUI : MonoBehaviour, IInitializable<ItemData>
{
    [SerializeField] private ItemData displayedItem;
    
    [SerializeField] private TextMeshProUGUI titleTmp;
    [SerializeField] private TextMeshProUGUI descriptionTmp;
    [SerializeField] private TextMeshProUGUI amountTmp;
    
    bool IInitializable<ItemData>.Initialized { get; set; }
    
    private InventorySystem _inventorySystem;
    

    private async void Awake() {
        await Toolbox.WaitUntilReadyAsync();
        _inventorySystem = Toolbox.Get<InventorySystem>();
        Init(displayedItem);
    }

    public void Init(ItemData data) {
        titleTmp.text = data.name;
        descriptionTmp.text = data.desc;
        amountTmp.text = _inventorySystem.GetAmount(data).ToString();
    }
}
