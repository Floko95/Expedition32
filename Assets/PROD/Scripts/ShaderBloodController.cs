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

            var materials = rend.materials; // Crée une copie instanciée des matériaux
            foreach (var mat in materials) {
                mat.SetFloat("_1_Blood_Intensity", bloodAmount);
                mat.SetFloat("_2_Blood_Intensity", bloodAmount);
                mat.SetFloat("_3_Blood_Intensity", bloodAmount);

                mat.SetFloat("_1_Dirt_Intensity", dirtAmount);
                mat.SetFloat("_2_Dirt_Intensity", dirtAmount);
                mat.SetFloat("_3_Dirt_Intensity", dirtAmount);
            }
            rend.materials = materials;
        }
    }
}
