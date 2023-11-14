using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    private RoadSpawner _roadSpawner;
    private float _score;

    private void FixedUpdate()
    {
        _score += 1;
        _scoreText.text = _score.ToString();
    }
}
