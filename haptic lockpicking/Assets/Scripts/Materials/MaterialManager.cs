using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    [Header("Renderers")]
    [SerializeField]
    MeshRenderer _coreRenderer;

    [SerializeField]
    MeshRenderer _hullRenderer;

    [SerializeField]
    List<MeshRenderer> _keyPinRenderers;


    [Header("Materials")]
    [SerializeField]
    List<Material> _corePresetMaterials;

    [SerializeField]
    List<Material> _hullPresetMaterials;

    [SerializeField]
    List<Material> _keyPinPresetMaterials;



    public void Update()
    {
        // "Presets"
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetCoreMaterial(_corePresetMaterials[0]);
            SetHullMaterial(_hullPresetMaterials[0]);
            SetKeyPinMaterial(_keyPinPresetMaterials[0]);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetCoreMaterial(_corePresetMaterials[1]);
            SetHullMaterial(_hullPresetMaterials[1]);
            SetKeyPinMaterial(_keyPinPresetMaterials[1]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetCoreMaterial(_corePresetMaterials[2]);
            SetHullMaterial(_hullPresetMaterials[2]);
            SetKeyPinMaterial(_keyPinPresetMaterials[2]);
        }
    }

    public void SetCoreMaterial(Material material)
    {
        _coreRenderer.sharedMaterial = material;
    }

    public void SetHullMaterial(Material material)
    {
        _hullRenderer.sharedMaterial = material;
    }

    public void SetKeyPinMaterial(Material material)
    {
        foreach (MeshRenderer keyPinRenderer in _keyPinRenderers)
        {
            keyPinRenderer.sharedMaterial = material;
        }
    }
}
