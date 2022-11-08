using Com.AndyBastel.Common.Ui;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    private static UiManager _instance;
    public static UiManager Instance => _instance;

    public static event Action onStartScreenClose; 
    public static event Action onWinScreenClose; 
    public static event Action onLooseScreenClose;

    [Header("Screens")]
    [SerializeField] private UiScreen _startScreen = null;
    [SerializeField] private UiScreen _winScreen = null;
    [SerializeField] private UiScreen _looseScreen = null;
    [SerializeField] private Hud _hudScreen = null;


    #region Unity Methods
    private void Awake()
    {
        if(_instance == null )
            _instance = this;
        else Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (_instance = this)
            _instance = null;
    }
    #endregion Unity Methods

    #region Screens
    public void DisplayStartScreen()
    {

        _startScreen.OnClose += StartScreen_OnClose;
        _startScreen.gameObject.SetActive(true);
    }

    public void DisplayWinScreen()
    {
        _winScreen.OnClose += WinScreen_OnClose;
        _winScreen.gameObject.SetActive(true);
    }


    public void DisplayLooseScreen()
    {
        _looseScreen.OnClose += LooseScreen_OnClose;
        _looseScreen.gameObject.SetActive(true);
    }

    public void DisplayHudScreen(int levelNb, int totalCoins)
    {
        _hudScreen.gameObject.SetActive(true);
        HudUpdateLevel(levelNb);
        HudUpdateCoins(totalCoins);
    }

    public void HideHudScreen()
    {
        _hudScreen.Close();
    }

    public void HudUpdateLevel(int newLevel)
    {
        _hudScreen.LevelText = newLevel.ToString();
    }

    public void HudUpdateCoins(int newCoinsAmount)
    {
        _hudScreen.CoinsText = newCoinsAmount.ToString();
    }

    #endregion Screens

    #region Events

    #region Callers
    private void InvokeOnStartScreenClose()
    {
        onStartScreenClose?.Invoke();
    }
    private void InvokeOnWinScreenClose()
    {
        onWinScreenClose?.Invoke();
    }
    private void InvokeOnLooseScreenClose()
    {
        onLooseScreenClose?.Invoke();
    }

    #endregion Callers

    #region Listeners
    private void StartScreen_OnClose(UiScreen sender)
    {
        _startScreen.OnClose -= StartScreen_OnClose;

        InvokeOnStartScreenClose();
    }
    private void WinScreen_OnClose(UiScreen sender)
    {
        _winScreen.OnClose -= WinScreen_OnClose;

        InvokeOnWinScreenClose();
    }
    private void LooseScreen_OnClose(UiScreen sender)
    {
        _looseScreen.OnClose -= LooseScreen_OnClose;

        InvokeOnLooseScreenClose();
    }

    #endregion Listeners

    #endregion Events




}
