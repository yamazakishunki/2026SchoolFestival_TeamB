using UnityEngine;
using System;
using System.Collections.Generic;

public class ActiveEffectManager : MonoBehaviour
{
    public static ActiveEffectManager Instance { get; private set; }

    public class ActiveEffect
    {
        public string Id;
        public Sprite Icon;
        public float Duration;
        public float Remaining;
        public Action OnExpire;
    }

    private const int MaxSlots = 3;
    private readonly List<ActiveEffect> activeEffects = new List<ActiveEffect>();

    public event Action OnEffectsChanged; // UI subscribes to this to refresh

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        bool changed = false;

        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            activeEffects[i].Remaining -= Time.deltaTime;
            if (activeEffects[i].Remaining <= 0f)
            {
                ExpireEffect(i);
                changed = true;
            }
        }

        if (changed) OnEffectsChanged?.Invoke();
    }

    // onActivate fires only the FIRST time this effect starts (or after it fully expires and starts fresh).
    // Re-picking up the same item while active only resets the timer — onActivate does NOT fire again.
    public void ActivateEffect(string id, Sprite icon, float duration, Action onActivate, Action onExpire)
    {
        ActiveEffect existing = activeEffects.Find(e => e.Id == id);

        if (existing != null)
        {
            existing.Duration = duration;
            existing.Remaining = duration; // reset timer only
            OnEffectsChanged?.Invoke();
            return;
        }

        if (activeEffects.Count >= MaxSlots)
        {
            // Evict the oldest (index 0 = earliest activated, since we always Add() to the end)
            activeEffects[0].OnExpire?.Invoke();
            activeEffects.RemoveAt(0);
        }

        var newEffect = new ActiveEffect
        {
            Id = id,
            Icon = icon,
            Duration = duration,
            Remaining = duration,
            OnExpire = onExpire
        };

        activeEffects.Add(newEffect);
        onActivate?.Invoke();
        OnEffectsChanged?.Invoke();
    }

    private void ExpireEffect(int index)
    {
        activeEffects[index].OnExpire?.Invoke();
        activeEffects.RemoveAt(index);
    }

    // ---- Read-only access for the UI ----
    public IReadOnlyList<ActiveEffect> GetActiveEffects() => activeEffects;
}