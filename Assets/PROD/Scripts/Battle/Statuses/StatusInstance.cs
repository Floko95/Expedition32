using System.Collections.Generic;

public class StatusInstance {
    
    public StatusData data;
    public Unit source;
    
    // Chaque stack a sa propre durée
    private List<int> stackDurations = new();

    public StatusInstance(StatusData data, Unit source, int duration, int stackCount = 1) {
        this.data = data;
        this.source = source;
        for (int i = 0; i < stackCount; i++) {
            stackDurations.Add(duration);
        }
    }

    public void AddStacks(int count, int duration) {
        for (int i = 0; i < count; i++) {
            stackDurations.Add(duration);
        }
    }

    public void Tick() {
        for (int i = 0; i < stackDurations.Count; i++) {
            stackDurations[i]--;
        }
        stackDurations.RemoveAll(d => d <= 0);
    }

    public bool IsExpired => stackDurations.Count == 0;

    public int StackCount => stackDurations.Count;
}