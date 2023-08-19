using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainChunk
{
    TerrainChunk[] children;
    ShapeGenerator shapeGenerator;
    Vector3 localPosition;
    float splitDistance;
    int maxLOD;
    int detailLevel;
    int filterResolution;
    Vector3 localUp;
    Vector3 localRight;
    Vector3 localForward;
    TerrainChunkObject chunkObject;
    float visibleAngle;

    public TerrainChunk(ShapeGenerator shapeGenerator, ShapeSettings settings, Vector3 localUp, int row, int col, int rootLOD, float splitDistance, float visibleAngle)
    {
        this.shapeGenerator = shapeGenerator;
        filterResolution = settings.meshFilterResolution;
        this.splitDistance = splitDistance;
        maxLOD = settings.levelOfDetail;
        detailLevel = 0;
        this.localUp = localUp;
        localRight = new Vector3(localUp.y, localUp.z, localUp.x);
        localForward = Vector3.Cross(localUp, localRight);
        localPosition = localUp + (((2f * row + 1f) / rootLOD) - 1f) * localRight + (((2f * col + 1f) / rootLOD) - 1f) * localForward;
        localRight /= rootLOD;
        localForward /= rootLOD;
        this.visibleAngle = visibleAngle;
    }

    public TerrainChunk(ShapeGenerator shapeGenerator, Vector3 localPosition, float splitDistance, int filterResolution, int maxLOD, int detailLevel, Vector3 localUp, Vector3 localRight, Vector3 localForward, float visibleAngle)
    {
        this.shapeGenerator = shapeGenerator;
        this.localPosition = localPosition;
        this.splitDistance = splitDistance;
        this.filterResolution = filterResolution;
        this.maxLOD = maxLOD;
        this.detailLevel = detailLevel;
        this.localUp = localUp;
        this.localRight = localRight;
        this.localForward = localForward;
        this.visibleAngle = visibleAngle;
    }

    public Vector3 GetPointOnCubeSphere(Vector2 percent)
    {
        Vector3 pointOnUnitCube = localPosition + (2f * (percent.x - 0.5f) * localRight + 2f * (percent.y - 0.5f) * localForward);
        float x2 = pointOnUnitCube.x * pointOnUnitCube.x;
        float y2 = pointOnUnitCube.y * pointOnUnitCube.y;
        float z2 = pointOnUnitCube.z * pointOnUnitCube.z;
        Vector3 pointOnUnitSphere;
        pointOnUnitSphere.x = pointOnUnitCube.x * Mathf.Sqrt(1f - y2 / 2f - z2 / 2f + y2 * z2 / 3f);
        pointOnUnitSphere.y = pointOnUnitCube.y * Mathf.Sqrt(1f - x2 / 2f - z2 / 2f + x2 * z2 / 3f);
        pointOnUnitSphere.z = pointOnUnitCube.z * Mathf.Sqrt(1f - x2 / 2f - y2 / 2f + x2 * y2 / 3f);
        return pointOnUnitSphere;

        /*Vector3 pointOnUnitCube = localPosition + (2f * (percent.x - 0.5f) * localRight + 2f * (percent.y - 0.5f) * localForward);
        return pointOnUnitCube.normalized;*/
    }

    public void ConstructMesh()
    {
        Mesh filterMesh = chunkObject.meshFilter.sharedMesh;
        Vector3[] vertices = new Vector3[filterResolution * filterResolution]; // resolution vertices on each side of the mesh
        int[] triangles = new int[(filterResolution - 1) * (filterResolution - 1) * 6]; // 2 triangles per square so (resolution - 1)^2 faces * 2 tris * 3 vertices per tri
        int triangleIndex = 0;
        Vector2[] uv = filterMesh.uv;

        int i = 0;
        for (int y = 0; y < filterResolution; y++)
        {
            for (int x = 0; x < filterResolution; x++)
            {
                vertices[i] = shapeGenerator.CalculatePointOnSphere(GetPointOnCubeSphere(new Vector2(x, y) / (filterResolution - 1)));

                if (x != filterResolution - 1 && y != filterResolution - 1)
                {
                    triangles[triangleIndex] = i;
                    triangles[triangleIndex + 1] = i + filterResolution + 1;
                    triangles[triangleIndex + 2] = i + filterResolution;

                    triangles[triangleIndex + 3] = i;
                    triangles[triangleIndex + 4] = i + 1;
                    triangles[triangleIndex + 5] = i + filterResolution + 1;
                    triangleIndex += 6;
                }
                i++;
            }
        }

        filterMesh.Clear();
        filterMesh.vertices = vertices;
        filterMesh.triangles = triangles;
        filterMesh.RecalculateNormals();
        if (filterMesh.uv.Length == uv.Length)
            filterMesh.uv = uv;        
    }

    public void UpdateUVs(ColorGenerator colorGenerator)
    {
        int verticesLength = filterResolution * filterResolution;
        Vector2[] uv = new Vector2[verticesLength];

        int i = 0;
        for (int y = 0; y < filterResolution; y++)
        {
            for (int x = 0; x < filterResolution; x++)
            {
                uv[i] = new Vector2(colorGenerator.BiomePercentFromPoint(GetPointOnCubeSphere(new Vector2(x, y) / (filterResolution - 1))), 0);
                i++;
            }
        }
        chunkObject.meshFilter.sharedMesh.uv = uv;
    }

    public void GenerateTree(Camera camera, Transform parent, ColorGenerator colorGenerator)
    {
        float sqrDistance = 0;
        if(camera != null)
            sqrDistance = chunkObject == null ? (parent.TransformPoint(shapeGenerator.CalculatePointOnSphere(GetPointOnCubeSphere(new Vector2(0.5f, 0.5f)))) - camera.transform.position).sqrMagnitude : chunkObject.meshRenderer.bounds.SqrDistance(camera.transform.position);
        if (camera != null && detailLevel >= 0 && detailLevel < maxLOD && sqrDistance < splitDistance * splitDistance)
        {
            DisableChunk();
            children = new TerrainChunk[4];
            float nextDistance = splitDistance * 0.5f;
            Vector3 nextLocalRight = localRight * 0.5f;
            Vector3 nextLocalForward = localForward * 0.5f;
            children[0] = new TerrainChunk(shapeGenerator, localPosition + nextLocalRight - nextLocalForward, nextDistance, filterResolution, maxLOD, detailLevel + 1, localUp, nextLocalRight, nextLocalForward, visibleAngle); // Top left
            children[1] = new TerrainChunk(shapeGenerator, localPosition + nextLocalRight + nextLocalForward, nextDistance, filterResolution, maxLOD, detailLevel + 1, localUp, nextLocalRight, nextLocalForward, visibleAngle); // Top right
            children[2] = new TerrainChunk(shapeGenerator, localPosition - nextLocalRight + nextLocalForward, nextDistance, filterResolution, maxLOD, detailLevel + 1, localUp, nextLocalRight, nextLocalForward, visibleAngle); // Bottom right
            children[3] = new TerrainChunk(shapeGenerator, localPosition - nextLocalRight - nextLocalForward, nextDistance, filterResolution, maxLOD, detailLevel + 1, localUp, nextLocalRight, nextLocalForward, visibleAngle); // Bottom left

            foreach (TerrainChunk chunk in children)
            {
                chunk.GenerateTree(camera, parent, colorGenerator);
            }
        }
        else
        {
            if (children != null)
            {
                foreach (TerrainChunk chunk in children)
                {
                    chunk.DisableChunk();
                }
            }
            if (chunkObject == null)
            {
                GameObject meshGO = new GameObject("Mesh (" + detailLevel + " LOD)");
                meshGO.transform.SetParent(parent, false);
                chunkObject = meshGO.AddComponent<TerrainChunkObject>();
                chunkObject.Init(this, visibleAngle);
                chunkObject.gameObject.layer = parent.gameObject.layer;
            }

            chunkObject.gameObject.SetActive(true);
            chunkObject.meshRenderer.sharedMaterial = colorGenerator.materialInstance;
            chunkObject.meshFilter.sharedMesh = new Mesh();
            if (camera != null)
            {
                UpdateUVs(colorGenerator);
            }
        }
    }

    /* 
     * Doesn't generate chunks already created and enables mesh depending on if mesh is visible by camera
     */
    public void UpdateTree(Camera camera, Transform parent, ColorGenerator colorGenerator)
    {
        float sqrDistance = 0;
        if (camera != null)
            sqrDistance = chunkObject == null ? (parent.TransformPoint(shapeGenerator.CalculatePointOnSphere(GetPointOnCubeSphere(new Vector2(0.5f, 0.5f)))) - camera.transform.position).sqrMagnitude : chunkObject.meshRenderer.bounds.SqrDistance(camera.transform.position);
        if (camera != null && detailLevel >= 0 && detailLevel < maxLOD && sqrDistance < splitDistance * splitDistance)
        {
            DisableChunk();
            if (children == null || children.Length == 0)
            {
                children = new TerrainChunk[4];
                float nextDistance = splitDistance * 0.5f;
                Vector3 nextLocalRight = localRight * 0.5f;
                Vector3 nextLocalForward = localForward * 0.5f;
                children[0] = new TerrainChunk(shapeGenerator, localPosition + nextLocalRight - nextLocalForward, nextDistance, filterResolution, maxLOD, detailLevel + 1, localUp, nextLocalRight, nextLocalForward, visibleAngle); // Top left
                children[1] = new TerrainChunk(shapeGenerator, localPosition + nextLocalRight + nextLocalForward, nextDistance, filterResolution, maxLOD, detailLevel + 1, localUp, nextLocalRight, nextLocalForward, visibleAngle); // Top right
                children[2] = new TerrainChunk(shapeGenerator, localPosition - nextLocalRight + nextLocalForward, nextDistance, filterResolution, maxLOD, detailLevel + 1, localUp, nextLocalRight, nextLocalForward, visibleAngle); // Bottom right
                children[3] = new TerrainChunk(shapeGenerator, localPosition - nextLocalRight - nextLocalForward, nextDistance, filterResolution, maxLOD, detailLevel + 1, localUp, nextLocalRight, nextLocalForward, visibleAngle); // Bottom left
            }

            foreach (TerrainChunk chunk in children)
            {
                chunk.UpdateTree(camera, parent, colorGenerator);
            }
        }
        else
        {
            if(children != null)
            {
                foreach(TerrainChunk chunk in children)
                {
                    chunk.DisableChunk();
                }
            }
            if (chunkObject == null)
            {
                GameObject meshGO = new GameObject("Mesh (" + detailLevel + " LOD)");
                meshGO.transform.SetParent(parent, false);
                chunkObject = meshGO.AddComponent<TerrainChunkObject>();
                chunkObject.Init(this, visibleAngle);
                chunkObject.gameObject.layer = parent.gameObject.layer;
                chunkObject.meshFilter.sharedMesh = new Mesh();
                chunkObject.meshRenderer.sharedMaterial = colorGenerator.materialInstance;
                ConstructMesh();
                UpdateUVs(colorGenerator);
            }
            chunkObject.gameObject.SetActive(true);
        }
    }

    private void DisableChunk()
    {
        if (chunkObject != null)
            chunkObject.gameObject.SetActive(false);
    }
}
