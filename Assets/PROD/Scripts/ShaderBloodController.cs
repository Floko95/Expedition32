using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ShaderBloodController : MonoBehaviour
{
    [SerializeField] private List<SkinnedMeshRenderer> renderers;
    [SerializeField, Range(0,10), OnValueChanged(nameof(UpdateShader))] private float bloodAmount;
    [SerializeField, Range(1,5), OnValueChanged(nameof(UpdateShader))] private float dirtAmount;

    public float BloodAmountNormalized {
        get {
            return bloodAmount;
        }
        set {
            bloodAmount = value * 10f;
            dirtAmount = value * 5f;
            UpdateShader();
        }
    }

    private Character _character;
    
    private void Start() {
        UpdateShader();
    }

    private void UpdateShader() {
        if(!Application.isPlaying) return;
        
        foreach (var rend in renderers) {
            rend.material.SetFloat("_1_Blood_Intensity", bloodAmount);
            rend.material.SetFloat("_2_Blood_Intensity", bloodAmount);
            rend.material.SetFloat("_3_Blood_Intensity", bloodAmount);
            
            rend.material.SetFloat("_1_Dirt_Intensity", dirtAmount);
            rend.material.SetFloat("_2_Dirt_Intensity", dirtAmount);
            rend.material.SetFloat("_3_Dirt_Intensity", dirtAmount);
        }
    }
}
