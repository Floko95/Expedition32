using System;
using System.Collections;
using UnityEngine;

public static class BehaviourExtensions
{
    public static Coroutine InvokeCallback(this MonoBehaviour that, float delay, Action lambda, bool unscaleTIme = false) {
        return that.StartCoroutine(_InvokeCallback(delay, lambda, unscaleTIme));
    }
    
    public static Coroutine InvokeInOneFrame(this MonoBehaviour that, Action lambda) {
        return that.StartCoroutine(_WaitOneFrame(lambda));
    }
    
    private static IEnumerator _InvokeCallback(float delay, Action lambda, bool unscaleTIme) {
        if (unscaleTIme == true) {
            yield return new WaitForSecondsRealtime(delay);
        } else {
            yield return new WaitForSeconds(delay);
        }
        lambda();
    }
    
    private static IEnumerator _WaitOneFrame(Action lambda) {
        yield return null;
        lambda();
    }
}
