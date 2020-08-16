using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChameleonRender : MonoBehaviour
{
    public ChameleonInstancingObj instancingObjs;
    public Material ChameleonInstancingMat;
    public int ActiveMesh = 0;

    private void Start()
    {
        var renderer = GetComponent<Renderer>();
        var meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null) meshFilter = gameObject.AddComponent<MeshFilter>();
        if (renderer == null) renderer = gameObject.AddComponent<MeshRenderer>();
        meshFilter.mesh = instancingObjs.finalMesh !=null?instancingObjs.finalMesh:new Mesh();
        renderer.sharedMaterial = ChameleonInstancingMat;
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_MeshID", ActiveMesh / (float) instancingObjs.meshs.Count);
        renderer.SetPropertyBlock(mpb);
    }

    private void OnRenderObject()
    {
        
    }

    private void LateUpdate()
    {
        
    }

}
