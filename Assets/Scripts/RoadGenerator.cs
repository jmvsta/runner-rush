using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public GameObject RoadPrefab;
    private List<GameObject> roads = new List<GameObject>();
    public float MaxSpeed = 10;
    private float speed = 0;
    public int MaxRoadCount = 5;

    // Start is called before the first frame update
    void Start()
    {
        ResetLevel();
        StartLevel();
    }

    public void StartLevel()
    {
        speed = MaxSpeed;
    }

    public void CreateNextRoad()
    {
        Vector3 pos = Vector3.zero;

        if (roads.Count > 0)
        {
            pos = roads[roads.Count - 1].transform.position + new Vector3(0, 0, 15);
        }

        GameObject newRoad = Instantiate(RoadPrefab, pos, Quaternion.identity);
        newRoad.transform.SetParent(transform);
        roads.Add(newRoad);
    }

    public void ResetLevel()
    {
        speed = 0;

        while (roads.Count > 0)
        {
            Destroy(roads[0]);
            roads.RemoveAt(0);
        }

        for (int i = 0; i < MaxRoadCount; i++)
        {
            CreateNextRoad();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (speed == 0)
        {
            return;
        }

        foreach (GameObject road in roads)
        {
            road.transform.position -= new Vector3(0, 0, speed * Time.deltaTime);
        }

        if (roads[0].transform.position.z < -15)
        {
            Destroy(roads[0]);
            roads.RemoveAt(0);

            CreateNextRoad();
        }
    }
}
