using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Utility;
using PathCreation;

public static class Tools3d 
{

public static GameObject CreateDrop(string name, Transform parent, Vector3 pos, Material mat){
        // Debug.Log("cube_"+ name);
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        obj.name = name;
        float sc = 0.3f;
        obj.transform.localScale = new Vector3(sc, sc, sc);
        if(pos != null){obj.transform.position = pos; }
        if(parent != null){obj.transform.parent = parent; }
        if(mat != null){obj.GetComponent<Renderer>().material = mat; }
        // cube.transform.position = new Vector3(0, 0.5f, 0);
        return obj;
    }
        public static Transform CreateDropSmall(string name, Transform parent, Vector3 pos, float sc, Material mat){
        // Debug.Log("cube_"+ name);
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        obj.name = name;
        obj.transform.localScale = new Vector3(sc, sc, sc);
        if(pos != null){obj.transform.position = pos; }
        obj.GetComponent<Renderer>().material = mat;
        if(parent != null){obj.transform.parent = parent; }
        // if(mat != null){obj.GetComponent<Renderer>().material = mat; }
        // cube.transform.position = new Vector3(0, 0.5f, 0);
        return obj.transform;
    }
	// PathCreation.VertexPath

	public static GameObject CreateMesh(string name, VertexPath path, float thickness = .02f, int resolutionU = 6, float resolutionV = 6, Material material = null)
	{
		GameObject obj = new GameObject(name);
		MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();

		List<Vector3> verts = new List<Vector3>();
		List<int> triangles = new List<int>();

		int numCircles = Mathf.Max(2, Mathf.RoundToInt(path.length * resolutionV) + 1);
		var pathInstruction = PathCreation.EndOfPathInstruction.Stop;
		
		for (int s = 0; s < numCircles; s++)
		{
			float segmentPercent = s / (numCircles - 1f);
			Vector3 centerPos = path.GetPointAtTime(segmentPercent, pathInstruction);
			Vector3 norm = path.GetNormal(segmentPercent, pathInstruction);
			Vector3 forward = path.GetDirection(segmentPercent, pathInstruction);
			Vector3 tangentOrWhatEver = Vector3.Cross(norm, forward);

			for (int currentRes = 0; currentRes < resolutionU; currentRes++)
			{
				var angle = ((float)currentRes / resolutionU) * (Mathf.PI * 2.0f);

				var xVal = Mathf.Sin(angle) * thickness;
				var yVal = Mathf.Cos(angle) * thickness;

				var point = (norm * xVal) + (tangentOrWhatEver * yVal) + centerPos;
				verts.Add(point);

				//! Adding the triangles
				if (s < numCircles - 1)
				{
					int startIndex = resolutionU * s;
					triangles.Add(startIndex + currentRes);
					triangles.Add(startIndex + (currentRes + 1) % resolutionU);
					triangles.Add(startIndex + currentRes + resolutionU);

					triangles.Add(startIndex + (currentRes + 1) % resolutionU);
					triangles.Add(startIndex + (currentRes + 1) % resolutionU + resolutionU);
					triangles.Add(startIndex + currentRes + resolutionU);
				}

			}
		}

		Mesh mesh = new Mesh();		
		mesh.SetVertices(verts);
		mesh.SetTriangles(triangles, 0);
		mesh.RecalculateNormals();
		meshFilter.sharedMesh = mesh;
		if (material != null){meshRenderer.sharedMaterial = material;}
		return obj;
	}

}
