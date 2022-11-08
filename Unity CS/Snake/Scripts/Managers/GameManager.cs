using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    private const int FRAME_RATE = 60;
    private int _levelNb = 0;
    private int _totalCoins = 0;


    #region Unity Methods
    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        SuscribeToEvents();
        Application.targetFrameRate = FRAME_RATE;
    }

    private void Start()
    {
        UiManager.Instance.DisplayStartScreen();
    }

    private void OnDestroy()
    {
        UnsuscribeToEvents();
        if(_instance == this)
            _instance = null;
    }
    #endregion Unity Methods

    #region Events

    #region Listeners
    private void SuscribeToEvents()
    {
        LevelManager.onWon += LevelManager_onWon;
        LevelManager.onLost += LevelManager_onLost;
        LevelManager.onCoinObtained += LevelManager_onCoinObtained;
        UiManager.onStartScreenClose += UiManager_onStartScreenClose;
        UiManager.onWinScreenClose += UiManager_onWinScreenClose;
        UiManager.onLooseScreenClose += UiManager_onLooseScreenClose;
    }

    private void UnsuscribeToEvents()
    {
        LevelManager.onWon              -= LevelManager_onWon;
        LevelManager.onLost             -= LevelManager_onLost;
        LevelManager.onCoinObtained     -= LevelManager_onCoinObtained;
        UiManager.onStartScreenClose    -= UiManager_onStartScreenClose;
        UiManager.onWinScreenClose      -= UiManager_onWinScreenClose;
        UiManager.onLooseScreenClose    -= UiManager_onLooseScreenClose;
    }
    private void LevelManager_onCoinObtained(LevelManager sender)
    {
        _totalCoins++;
        UiManager.Instance.HudUpdateCoins(_totalCoins);
    }


    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        UiManager.Instance.DisplayStartScreen();
    }

    private void UiManager_onStartScreenClose()
    {
        if(LevelManager.Instance != null)
            LevelManager.Instance.StartLevel();

        if (UiManager.Instance != null)
            UiManager.Instance.DisplayHudScreen(_levelNb , _totalCoins);
    }
    private void UiManager_onLooseScreenClose()
    {
        ReloadLevel();
    }

    private void UiManager_onWinScreenClose()
    {
        ReloadLevel();
    }


    private void LevelManager_onLost(LevelManager sender)
    {
        UiManager lInstance = UiManager.Instance;
        if (lInstance != null)
        {
            lInstance.HideHudScreen();
            lInstance.DisplayLooseScreen();
        }

        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayLooseSound();
    }

    private void LevelManager_onWon(LevelManager sender)
    {
        UiManager lInstance = UiManager.Instance;
        _levelNb++;

        if (lInstance != null)
        {
            lInstance.HudUpdateLevel(_levelNb);
            lInstance.HudUpdateCoins(_totalCoins);
            lInstance.HideHudScreen();

            UiManager.Instance.DisplayWinScreen();

        }


        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayWinSound();
    }
    #endregion Listeners

    #endregion Events

    #region Flow
    private void ReloadLevel()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        SceneManager.LoadSceneAsync(0);
    }
    #endregion



}
