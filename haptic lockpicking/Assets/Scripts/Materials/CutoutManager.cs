using System.Collections.Generic;
using UnityEngine;

public class CutoutManager : MonoBehaviour
{
    [Header("Core")]
    [SerializeField]
    GameObject _core;

    [SerializeField]
    GameObject _coreBackHalf;

    [SerializeField]
    GameObject _coreFrontHalf;

    [Header("Hull")]
    [SerializeField]
    GameObject _hull;

    [SerializeField]
    GameObject _hullBackHalf;

    [SerializeField]
    GameObject _hullFrontHalf;

    public void Start()
    {
        SetPreset1();
    }

    public void SetPreset1()
    {
        _core.SetActive(true);
        _coreBackHalf.SetActive(false);
        _coreFrontHalf.SetActive(false);

        _hull.SetActive(true);
        _hullBackHalf.SetActive(false);
        _hullFrontHalf.SetActive(false);
    }

    public void SetPreset2()
    {
        _core.SetActive(true);
        _coreBackHalf.SetActive(false);
        _coreFrontHalf.SetActive(false);

        _hull.SetActive(false);
        _hullBackHalf.SetActive(true);
        _hullFrontHalf.SetActive(true);
    }
    public void SetPreset3()
    {
        _core.SetActive(false);
        _coreBackHalf.SetActive(true);
        _coreFrontHalf.SetActive(true);

        _hull.SetActive(false);
        _hullBackHalf.SetActive(true);
        _hullFrontHalf.SetActive(true);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetPreset1();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetPreset2();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetPreset3();
        }
    }




    //[Header("Renderers")]
    //[SerializeField]
    //MeshRenderer _coreRenderer;

    //[SerializeField]
    //MeshRenderer _hullRenderer;

    //[SerializeField]
    //List<MeshRenderer> _keyPinRenderers;


    //[Header("Materials")]
    //[SerializeField]
    //List<Material> _corePresetMaterials;

    //[SerializeField]
    //List<Material> _hullPresetMaterials;

    //[SerializeField]
    //List<Material> _keyPinPresetMaterials;



    //public void Update()
    //{
    //    // "Presets"
    //    if (Input.GetKeyDown(KeyCode.Alpha1))
    //    {
    //        SetCoreMaterial(_corePresetMaterials[0]);
    //        SetHullMaterial(_hullPresetMaterials[0]);
    //        SetKeyPinMaterial(_keyPinPresetMaterials[0]);

    //    }
    //    else if (Input.GetKeyDown(KeyCode.Alpha2))
    //    {
    //        SetCoreMaterial(_corePresetMaterials[1]);
    //        SetHullMaterial(_hullPresetMaterials[1]);
    //        SetKeyPinMaterial(_keyPinPresetMaterials[1]);
    //    }
    //    else if (Input.GetKeyDown(KeyCode.Alpha3))
    //    {
    //        SetCoreMaterial(_corePresetMaterials[2]);
    //        SetHullMaterial(_hullPresetMaterials[2]);
    //        SetKeyPinMaterial(_keyPinPresetMaterials[2]);
    //    }
    //}

    //public void SetCoreMaterial(Material material)
    //{
    //    _coreRenderer.sharedMaterial = material;
    //}

    //public void SetHullMaterial(Material material)
    //{
    //    _hullRenderer.sharedMaterial = material;
    //}

    //public void SetKeyPinMaterial(Material material)
    //{
    //    foreach (MeshRenderer keyPinRenderer in _keyPinRenderers)
    //    {
    //        keyPinRenderer.sharedMaterial = material;
    //    }
    //}
}
