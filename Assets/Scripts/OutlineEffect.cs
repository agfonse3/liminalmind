using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class OutlineEffect : MonoBehaviour
{
    public Material outlineMaterial;

    void Start()
    {
        GameObject outlineObj = new GameObject("Outline");
        outlineObj.transform.parent = transform;
        outlineObj.transform.localPosition = Vector3.zero;
        outlineObj.transform.localRotation = Quaternion.identity;
        outlineObj.transform.localScale = Vector3.one;

        var meshFilter = outlineObj.AddComponent<MeshFilter>();
        meshFilter.sharedMesh = GetComponent<MeshFilter>().sharedMesh;

        var meshRenderer = outlineObj.AddComponent<MeshRenderer>();
        meshRenderer.material = outlineMaterial;
    }
}

