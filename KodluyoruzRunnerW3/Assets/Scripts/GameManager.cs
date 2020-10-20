using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Slider _processBar;
    [SerializeField] private ParticleSystem[] _finishParticle;
    private static GameManager _instance;
    private GameState _gameState;
    public bool gameStarted { get; private set; }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        DontDestroyOnLoad(this);
    }
    public static GameManager Instance()
    {
        return _instance;
    }
    private void Start()
    {
        gameStarted = true;
        PrepareGame();
    }
    private void PrepareGame()
    {
        _gameState = new GameState();
        _gameState.totalCheckPoint = 6;
        ChangProcessValue();
    }
    public void ChangeCheckPoint(int id)
    {
        _gameState.currentCheckPoint = id + 1;
        if(id +1 == _gameState.totalCheckPoint)
        {
            EndLevel();
        }
        ChangProcessValue();
    }

    private void EndLevel()
    {
        foreach(var item in _finishParticle)
        {
            var emission = item.emission;
            emission.enabled = true;
        }
    }

    private void ChangProcessValue()
    {
        float processValue = (float)_gameState.currentCheckPoint / (float)_gameState.totalCheckPoint;
        _processBar.value = processValue;
    }
}
