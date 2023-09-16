using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public GameObject RoadPrefab;
    private List<GameObject> _roads = new List<GameObject>();
    public float MaxSpeed = 10;
    private float _speed = 0;
    public int MaxRoadCount = 10;

    // Start is called before the first frame update
    void Start()
    {
        ResetLevel();
    }

    public void StartLevel()
    {
        _speed = MaxSpeed;
        SwipeManager.instance.enabled = true;
    }

    public void CreateNextRoad()
    {
        Vector3 _pos = Vector3.zero;

        if (_roads.Count > 0)
        {
            _pos = _roads[_roads.Count - 1].transform.position + new Vector3(0, 0, 30);
        }

        GameObject newRoad = Instantiate(RoadPrefab, _pos, Quaternion.identity);
        newRoad.transform.SetParent(transform);
        _roads.Add(newRoad);
    }

    public void ResetLevel()
    {
        _speed = 0;

        while (_roads.Count > 0)
        {
            Destroy(_roads[0]);
            _roads.RemoveAt(0);
        }

        for (int i = 0; i < MaxRoadCount; i++)
        {
            CreateNextRoad();
        }

        SwipeManager.instance.enabled = true;
    }

    void Update()
    {
        if (_speed == 0)
        {
            return;
        }

        foreach (GameObject road in _roads)
        {
            road.transform.position -= new Vector3(0, 0, _speed * Time.deltaTime);
        }

        if (_roads[0].transform.position.z < -30)
        {
            Destroy(_roads[0]);
            _roads.RemoveAt(0);

            CreateNextRoad();
        }
    }
}
