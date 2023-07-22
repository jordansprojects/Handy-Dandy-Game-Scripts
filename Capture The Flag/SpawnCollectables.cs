using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class SpawnCollectables : MonoBehaviour
{
	[SerializeField] List<Transform> limits;
	[SerializeField] int spawnCount =  7;
	List<Vector2> convexHull;
	[SerializeField] GameObject collectablePrefab;
	[SerializeField] UnityEvent spawningCompleteEvent;

	[SerializeField] GrahamScanner grahamScanner; /* to generate convex hull */
	// Start is called before the first frame update
	void Start(){
		if (limits.Count < 3){
			Debug.LogWarning("Need at least 3 points to calculate the 2D region.");
			return;
		}
		convexHull = grahamScanner.GrahamScan(limits);
		Spawn();
	}

	// Update is called once per frame
	void Update()
	{

	}

	void Spawn(){
			// Calculate the bounding box of the convex hull
			Vector2 minPoint = convexHull[0];
			Vector2 maxPoint = convexHull[0];
			foreach (Vector2 point in convexHull)
			{
				minPoint = Vector2.Min(minPoint, point);
				maxPoint = Vector2.Max(maxPoint, point);
			}

			for (int i = 0; i < spawnCount; i++)
			{
				Vector2 randomPoint = new Vector2(Random.Range(minPoint.x, maxPoint.x), Random.Range(minPoint.y, maxPoint.y));
				while (!grahamScanner.IsPointInConvexHull(randomPoint, convexHull))
				{
					randomPoint = new Vector2(Random.Range(minPoint.x, maxPoint.x), Random.Range(minPoint.y, maxPoint.y));
				}

				// Use the randomPoint as needed (e.g., spawn an object at that position)
				Instantiate(collectablePrefab, new Vector2( randomPoint.x , randomPoint.y ), Quaternion.identity);
			}

			// invoke spawning completion event 
			spawningCompleteEvent.Invoke();

	}
}
