using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArkanoidController : MonoBehaviour
{
    private const string BALL_PREFAB_PATH = "Prefabs/Ball";
    private readonly Vector2 BALL_INIT_POSITION = new Vector2(0, -0.86f);
    
    [SerializeField]
    private GridController _gridController;
    
    [Space(20)]
    [SerializeField]
    private List<LevelData> _levels = new List<LevelData>();
    
    private int _currentLevel = 1;
    private int _totalScore = 0;
    
    private Ball _ballPrefab = null;

    public GameObject _score50;
    public GameObject _score100;
    public GameObject _score250;
    public GameObject _score500;
    public GameObject _fast;
    public GameObject _slow;
    public GameObject _large;
    public GameObject _small;
    private List<Ball> _balls = new List<Ball>();
    
    
    private void Start()
    {
        ArkanoidEvent.OnBallReachDeadZoneEvent += OnBallReachDeadZone;
        ArkanoidEvent.OnBlockDestroyedEvent += OnBlockDestroyed;
    }

    private void OnDestroy()
    {
        ArkanoidEvent.OnBallReachDeadZoneEvent -= OnBallReachDeadZone;
        ArkanoidEvent.OnBlockDestroyedEvent -= OnBlockDestroyed;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InitGame();
        }
    }
    
    private void InitGame()
    {
        _totalScore = 0;
        
        _gridController.BuildGrid(_levels[0]);
        SetInitialBall();
        
        ArkanoidEvent.OnGameStartEvent?.Invoke();
        ArkanoidEvent.OnScoreUpdatedEvent?.Invoke(0, _totalScore);
        ArkanoidEvent.OnLevelUpdatedEvent?.Invoke(1, _currentLevel);
    }
    
    private void SetInitialBall()
    {
        ClearBalls();

        Ball ball = CreateBallAt(BALL_INIT_POSITION);
        ball.Init();
        _balls.Add(ball);
    }
    
    private Ball CreateBallAt(Vector2 position)
    {
        if (_ballPrefab == null)
        {
            _ballPrefab = Resources.Load<Ball>(BALL_PREFAB_PATH);
        }

        return Instantiate(_ballPrefab, position, Quaternion.identity);
    }
    
    private void ClearBalls()
    {
        for (int i = _balls.Count - 1; i >= 0; i--)
        {
            _balls[i].gameObject.SetActive(false);
            Destroy(_balls[i]);
        }
        
        _balls.Clear();
    }
    
    private void OnBallReachDeadZone(Ball ball)
    {
        ball.Hide();
        _balls.Remove(ball);
        Destroy(ball.gameObject);

        CheckGameOver();
    }
    
    private void CheckGameOver()
    {
        if (_balls.Count == 0)
        {
            //Game over
            ClearBalls();
            
            Debug.Log("Game Over: LOSE!!!");
            ArkanoidEvent.OnGameOverEvent?.Invoke();
        }
    }
    
    private void OnBlockDestroyed(int blockId)
    {
        
        BlockTile blockDestroyed = _gridController.GetBlockBy(blockId);
        if (blockDestroyed != null)
        {
            float blockX = blockDestroyed.transform.position.x;
            float blockY = blockDestroyed.transform.position.y;
            float valorRandom = Random.value;
            if(valorRandom > 0.5f){
                float valorRandom2 = Random.value;
                if (valorRandom2>0.5f)
                {
                    Spawn(blockX,blockY,50);  
                }else
                {
                   float valorRandom3 = Random.value;
                    if (valorRandom3>0.5f)
                    {
                        Spawn1(blockX,blockY,1);  
                    }else
                    {
                        Spawn1(blockX,blockY,0);
                    } 
                }
            } else if(valorRandom > 0.25f){
                float valorRandom2 = Random.value;
                if (valorRandom2>0.5f)
                {
                    Spawn(blockX,blockY,100);  
                }else
                {
                   float valorRandom3 = Random.value;
                    if (valorRandom3>0.5f)
                    {
                        Spawn2(blockX,blockY,1);  
                    }else
                    {
                        Spawn2(blockX,blockY,0);
                    } 
                }
            } else if(valorRandom > 0.15f){
                Spawn(blockX,blockY,250);
            } else if(valorRandom < 0.1f){
                Spawn(blockX,blockY,500);
            }
            
            _totalScore += blockDestroyed.Score;
            ArkanoidEvent.OnScoreUpdatedEvent?.Invoke(blockDestroyed.Score, _totalScore);
        }
        
        
        if (_gridController.GetBlocksActive() == 0)
        {
            if (_currentLevel >= _levels.Count)
            {
                ClearBalls();
                Debug.LogError("Game Over: WIN!!!!");
                _currentLevel++;
                ArkanoidEvent.OnLevelUpdatedEvent?.Invoke(1, _currentLevel);
            }
            else
            {
                SetInitialBall();
                _gridController.BuildGrid(_levels[_currentLevel]);
            }

        }
    }

    private void Spawn(float x, float y, int score){
        Vector2 pos = new Vector2(x,y);
        if (score==50)
        {
            Instantiate(_score50, pos, Quaternion.identity);
        }else if(score==100){
            Instantiate(_score100, pos, Quaternion.identity);
        }else if(score==250){
            Instantiate(_score250, pos, Quaternion.identity);
        }else if(score==500){
            Instantiate(_score500, pos, Quaternion.identity);
        }
    }
    private void Spawn1(float x, float y, int type){
        Vector2 pos = new Vector2(x,y);
        if (type==1)
        {
            Instantiate(_fast, pos, Quaternion.identity);
        }else{
            Instantiate(_slow, pos, Quaternion.identity);
        }
    }
    private void Spawn2(float x, float y, int type){
        Vector2 pos = new Vector2(x,y);
        if (type==1)
        {
            Instantiate(_large, pos, Quaternion.identity);
        }else{
            Instantiate(_small, pos, Quaternion.identity);
        }
    }

    public void plusScore(int score){
        _totalScore+=score;
    }
    public int getScore(){
        return _totalScore;
    }
}
