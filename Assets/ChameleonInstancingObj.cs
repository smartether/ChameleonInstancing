using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// solution 1: put tag meshID on mesh texcoord
/// solution 2: distinct meshID by vertexID in vertexProgram
/// </summary>

[ExecuteInEditMode()]
public class ChameleonInstancingObj : MonoBehaviour
{
    public Renderer[] renders;

    public readonly List<Mesh> meshs = new List<Mesh>();
    public Mesh finalMesh = null;
    
    // Start is called before the first frame update
    void Awake()
    {
        foreach(var render in renders)
        {
            var sharedMesh = render.GetComponent<MeshFilter>().sharedMesh;
            if (!meshs.Contains(sharedMesh))
            {
                meshs.Add(sharedMesh);
            }
        }

        List<CombineInstance> combineInsts = new List<CombineInstance>();
        for(int meshIdx=0,meshMax=meshs.Count;meshIdx<meshMax;meshIdx++)
        {
            var mesh = meshs[meshIdx];
            var tagUV = new Vector2[mesh.vertices.Length];
            for(int i = 0, c = mesh.vertices.Length; i < c; i++)
            {
                float meshId = meshIdx / (float)meshMax;
                tagUV[i] = new Vector2(meshId, meshId);
            }
            mesh.uv3 = tagUV;
            var matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
            var combineInst = new CombineInstance();
            combineInst.mesh = mesh;
            combineInst.transform = matrix;            
            combineInsts.Add(combineInst);
        }
        Mesh meshCombined  = new Mesh();
        meshCombined.CombineMeshes(combineInsts.ToArray());
        meshCombined.UploadMeshData(false);
        finalMesh = meshCombined;
    }

    
    // Update is called once per frame
    void Update()
    {

    }
}
