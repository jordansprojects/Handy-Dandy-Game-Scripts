using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GrahamScanner : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public List<Vector2> GrahamScan(List<Transform> inputPoints){
		// Convert the transforms to Vector2 points for easier calculation
		List<Vector2> points = new List<Vector2>();
		foreach (var point in inputPoints)
		{
			points.Add(new Vector2(point.position.x, point.position.y));
		}

		// Find the point with the lowest y-coordinate (and leftmost if there are ties)
		Vector2 pivot = points[0];
		foreach (var point in points)
		{
			if (point.y < pivot.y || (point.y == pivot.y && point.x < pivot.x))
			{
				pivot = point;
			}
		}
		// Sort the points by polar angle with respect to the pivot
		points.Sort((a, b) =>
				{
				float angleA = Mathf.Atan2(a.y - pivot.y, a.x - pivot.x);
				float angleB = Mathf.Atan2(b.y - pivot.y, b.x - pivot.x);
				if (angleA < angleB) return -1;
				if (angleA > angleB) return 1;
				return 0;
				});

		// Graham scan algorithm
		List<Vector2> convexHull = new List<Vector2>();
		convexHull.Add(points[0]);
		convexHull.Add(points[1]);

		for (int i = 2; i < points.Count; i++)
		{
			while (convexHull.Count >= 2 && CrossProduct(convexHull[convexHull.Count - 2], convexHull[convexHull.Count - 1], points[i]) <= 0)
			{
				convexHull.RemoveAt(convexHull.Count - 1);
			}

			convexHull.Add(points[i]);
		}

		return convexHull;
	}



	public float CrossProduct(Vector2 a, Vector2 b, Vector2 c){
		return (b.x - a.x) * (c.y - a.y) - (b.y - a.y) * (c.x - a.x);
	}


	public bool IsPointInConvexHull(Vector2 point, List<Vector2> convexHull){
		int j = convexHull.Count - 1;
		bool isInside = false;
		for (int i = 0; i < convexHull.Count; i++)
		{
			if ((convexHull[i].y < point.y && convexHull[j].y >= point.y || convexHull[j].y < point.y && convexHull[i].y >= point.y)
					&& (convexHull[i].x + (point.y - convexHull[i].y) / (convexHull[j].y - convexHull[i].y) * (convexHull[j].x - convexHull[i].x) < point.x))
			{
				isInside = !isInside;
			}
			j = i;
		}
		return isInside;
	}
}
