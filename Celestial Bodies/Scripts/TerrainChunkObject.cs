using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TerrainChunkObject : MonoBehaviour
{
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    [SerializeField] float visibleAngle;
    //Vector3 midPoint;
    Transform midPoint;

    private void Start()
    {
        midPoint = transform.Find("Mid Point");
    }

    private void Update()
    {
        UpdateMeshRenderer();
    } 

    public void Init(TerrainChunk chunk, float visibleAngle)
    {
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        this.visibleAngle = visibleAngle;
        //midPoint = chunk.GetPointOnCubeSphere(new Vector2(0.5f, 0.5f));
        midPoint = new GameObject("Mid Point").transform;
        midPoint.SetParent(transform);
        midPoint.localPosition = chunk.GetPointOnCubeSphere(new Vector2(0.5f, 0.5f));
    }

    private bool IsVisibleFrom(Camera camera)
    {
        //Vector3 globalMidPoint = transform.TransformPoint(midPoint);
        Vector3 normal = midPoint.position - transform.position;
        Vector3 toCamera = camera.transform.position - midPoint.position;
        //Debug.DrawRay(globalMidPoint, normal, Color.red, 0.1f);
        //Debug.DrawRay(globalMidPoint, toCamera, Color.green, 0.1f);
        if(Vector3.Angle(normal, toCamera) < visibleAngle)
        {
            if (meshRenderer.enabled)
            {
                return meshRenderer.isVisible;
            }
            else
            {
                foreach (Camera cameraInStack in camera.GetUniversalAdditionalCameraData().cameraStack)
                {
                    Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(cameraInStack);
                    if (GeometryUtility.TestPlanesAABB(frustumPlanes, meshRenderer.bounds))
                        return true;
                }
            }
        }
        return false;
    }

    private void UpdateMeshRenderer()
    {
        meshRenderer.enabled = IsVisibleFrom(Camera.main);
    }
}
