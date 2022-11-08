using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void LevelManagerEventHandler(LevelManager sender);

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;
    public static LevelManager Instance => _instance;


    public static event LevelManagerEventHandler onWon;
    public static event LevelManagerEventHandler onLost;
    public static event LevelManagerEventHandler onCoinObtained;

    [Header("3C")]
    [SerializeField] private InputManager _inputManager = null;
    [SerializeField] private Transform _snakeHeadPrefab = null;
    [SerializeField] private GameObject _cameraPrefab = null;

    [Header("LD")]
    [SerializeField] private Transform _spawnPoint = null;
    [SerializeField] private List<BlockSettings> _blocksSettings = new List<BlockSettings>();
    [SerializeField] private int _nbOfBlocks = 7;
    [SerializeField] private Vector3 _firstBlockPos = Vector3.zero;
    [SerializeField] private Transform _firstBlockPrefab = null;
    [SerializeField] private Transform _endBlockPrefab = null;

    [Header("Vibrations (in Milliseconds)")]
    [SerializeField, Range(5, 1000)] private long _foodVibrationTime = 15;
    [SerializeField, Range(5, 1000)] private long _trapVibrationTime = 15;
    [SerializeField, Range(5, 1000)] private long _portalVibrationTime = 20;
    [SerializeField, Range(5, 1000)] private long _coinVibrationTime = 10;
    [SerializeField, Range(5, 1000)] private long _winLooseVibrationTime = 50;


    private SnakeController _snakeController;
    private GameCamera _gameCamera;


    private List<Transform> _blocks = new List<Transform>();

    // GAME RULES
    // Permit to determine what was the previous block type
    private BlockType _previousBlockType = BlockType.Neutral;
    // Permit to know if the level manager has at least instianted one bonus block
    private bool _hasSpawnedBonusBlock = false;

    // FLOW
    private bool _wonLevel = false;
    private bool _lostLevel = false;

    #region Unity Methods

    // Start is called before the first frame update
    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        _inputManager.onFingerMoved += InputManager_onFingerMoved;

        Food.onAte += Food_onAte;
        Trap.onHit += Trap_onHit;
        Portal.onHit += Portal_onHit;
        Coin.onHit += Coin_onHit;
        BlockEnd.onFinishLineHit += BlockEnd_onFinishLineHit;
    }

    private void Start()
    {
        GeneratePath();
        InstantiateSnake();
        InstantiateCamera();

    }
    private void OnDestroy()
    {
        if (_instance == this) _instance = null;

        Food.onAte -= Food_onAte;
        Trap.onHit -= Trap_onHit;
        Portal.onHit -= Portal_onHit;
        Coin.onHit -= Coin_onHit;
        BlockEnd.onFinishLineHit -= BlockEnd_onFinishLineHit;
    }
    #endregion

    #region Instantiation
    private void GeneratePath()
    {
        int lIndex;
        int lBlockIndex = 0;
        Transform lBlock;
        for (lIndex = 0; lIndex < _nbOfBlocks; ++lIndex)
        {
            // In order to have the two firsts blocks normal
            if (lIndex <= 1)
                lBlock = Instantiate(_firstBlockPrefab);

            // If it's the last one
            else if (lIndex == _nbOfBlocks - 1)
                lBlock = Instantiate(_endBlockPrefab);

            // Otherwise choose a random block
            else
            {
                lBlockIndex = ChooseRandomBlock();
                lBlock = Instantiate(_blocksSettings[lBlockIndex].prefab);
            }

            lBlock.position = lIndex == 0 ? _firstBlockPos : GetPreviousBlockEndPos(lIndex);


            _blocks.Add(lBlock);
            _previousBlockType = _blocksSettings[lBlockIndex].type;
        }

    }

    private Vector3 GetPreviousBlockEndPos(int index)
    {
        return _blocks[index - 1].GetComponent<Block>().EndPos;
    }

    private int ChooseRandomBlock()
    {
        float lValue = UnityEngine.Random.Range(0f, 1f);
        int lIndex = 0;

        bool lIsMalus;
        bool lTwoMalusInARow;
        bool lMalusBeforeFirstBonus;

        foreach (BlockSettings blockSettings in _blocksSettings)
        {
            if (lValue >= blockSettings.luckFork.x && lValue < blockSettings.luckFork.y)
            {
                lIsMalus = blockSettings.type == BlockType.Malus;

                // Which means it would spawn two malus blocks in a row, we want to avoid this situation
                lTwoMalusInARow = lIsMalus && _previousBlockType == BlockType.Malus;

                // Which means a bonus block hasn't been spawned yet and the player could loose immediatly
                lMalusBeforeFirstBonus = lIsMalus && !_hasSpawnedBonusBlock;

                if (blockSettings.type == BlockType.Bonus && !_hasSpawnedBonusBlock)
                    _hasSpawnedBonusBlock = true;

                if (lTwoMalusInARow || lMalusBeforeFirstBonus)
                    return ChooseRandomBlock();

                // Else the situation is what we're expecting
                return lIndex;
            }

            lIndex++;
        }

        //If we haven't find any block, spawn the first one
        return 0;
    }


    private void InstantiateSnake()
    {
        // If there is no spawn point, instantiate it at the origin
        Vector3 lSpawnPos = _spawnPoint != null ? _spawnPoint.position : Vector3.zero;
        Transform lSnake = Instantiate(_snakeHeadPrefab);
        _snakeController = lSnake.GetComponent<SnakeController>();

        lSnake.position = lSpawnPos;

        _snakeController.onLost += SnakeController_onLost;
    }

    private void InstantiateCamera()
    {
        _gameCamera = Instantiate(_cameraPrefab).GetComponent<GameCamera>();

        _gameCamera.Target = _snakeController.transform;

        _inputManager.GameCamera = _gameCamera.GetComponentInChildren<Camera>();
    }

    #endregion Instantiation

    #region Flow
    private void WonGame()
    {
        InvokeOnWon();
        Vibrator.Vibrate(_winLooseVibrationTime);
    }

    private void LostGame()
    {
        Vibrator.Vibrate(_winLooseVibrationTime);
        _lostLevel = true;
        InvokeOnLost();
    }

    public void StartLevel()
    {
        _snakeController.StartMove();
    }
    #endregion Flow

    #region Game Mechanics
    private void GainParts(int partsGained = 1)
    {
        _snakeController.IncreaseSize(partsGained);
        _gameCamera.IncreaseDistance(partsGained);
    }

    private void LooseParts(int partsLost = 1)
    {
        if(_snakeController)
            _snakeController.DecreaseSize(partsLost);
        if(_gameCamera)
            _gameCamera.DecreaseDistance(partsLost);
    }

    #endregion Game Mechanics

    #region Events

    #region Callers

    private void InvokeOnWon()
    {
        onWon?.Invoke(this);
    }

    private void InvokeOnLost()
    {
        onLost?.Invoke(this);
    }
    private void InvokeOnCoinObtained()
    {
        onCoinObtained?.Invoke(this);
    }
    #endregion Callers

    #region Listeners

    private void SnakeController_onLost()
    {
        _snakeController.onLost -= SnakeController_onLost;

        if (!_wonLevel)
            LostGame();
        else WonGame();
    }
    private void Coin_onHit()
    {
        InvokeOnCoinObtained();
        Vibrator.Vibrate(_coinVibrationTime);
    }


    private void BlockEnd_onFinishLineHit()
    {
        if (_lostLevel) return;

        _snakeController.Passive();
        _wonLevel = true;
        WonGame();
    }

    private void Portal_onHit(Portal portal)
    {
        Portal lPortal = portal;

        if (lPortal.IsBonus)
        {
            if (lPortal.IsAddition)
                GainParts(lPortal.ImpactValue);
            else GainParts((lPortal.ImpactValue - 1) * _snakeController.BodyPartsNb);
        }
        else
        {
            if (lPortal.IsAddition)
                LooseParts(lPortal.ImpactValue);
            else LooseParts((int)(_snakeController.BodyPartsNb * ((float)(lPortal.ImpactValue - 1) / lPortal.ImpactValue)));
        }

        Vibrator.Vibrate(_portalVibrationTime);
    }

    private void Trap_onHit()
    {
        LooseParts();
        Vibrator.Vibrate(_trapVibrationTime);
    }

    private void Food_onAte()
    {
        GainParts();
        Vibrator.Vibrate(_foodVibrationTime);
    }
    private void InputManager_onFingerMoved(Vector2 delta)
    {
        _snakeController.ApplyMoveSideways(delta);
    }
    #endregion Listeners

    #endregion Events
}
