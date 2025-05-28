using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class InventorySystem : SerializedMonoBehaviour {
    [SerializeField] private Dictionary<ItemData, int> defaultInventory;
    
    public int GetAmount(ItemData item) => _items.ContainsKey(item) ? _items[item] : 0;
    
    private Dictionary<ItemData, int> _items = new Dictionary<ItemData, int>();

    
    public void Use(ItemData item, Unit user, Unit target) {
        if(_items.ContainsKey(item) == false || _items[item] == 0) return;
        item.Use(user, target);
    }

    private async void Awake() {
        await Toolbox.WaitUntilReadyAsync();
        Toolbox.Set(this);

        _items = defaultInventory;
    }
}
