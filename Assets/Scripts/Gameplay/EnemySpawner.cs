using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public int CurrentWave;
    public int EnemiesLeft = 20;
    [SerializeField] private Transform _leftSpawnPoint, _rightSpawnPoint;
    [SerializeField] private Transform _leftPivot, _rightPivot;
    [SerializeField] private Transform _enemyParent;
    [SerializeField] private List<Transform> _enemyPath;
    [SerializeField] private List<Enemy> _enemyList;
    [SerializeField] private float _timeMove = 0.6f;
    [SerializeField] private GameObject _enemyBoard;
    [SerializeField] private Vector3 _localScale = new Vector3(0.8f, 0.8f, 0.8f);
    private readonly WaitForSeconds _wait = new WaitForSeconds(0.15f);
    private readonly WaitForSeconds _timeDelay = new WaitForSeconds(0.5f);
    private readonly WaitForSeconds _timeNewWave = new WaitForSeconds(1.4f);
    
    [Header("Setup Movement")]
    [SerializeField] private float _time = 1f;
    [SerializeField] private Transform _startPos;
    [SerializeField] private Transform _endPos;

    private void OnEnable()
    {
        GameEventManager.CheckNewWave += CheckNewWave;
        GameEventManager.ReturnPoolObjects += ReturnObjects;
    }
    
    private void OnDisable()
    {
        GameEventManager.CheckNewWave -= CheckNewWave;
        GameEventManager.ReturnPoolObjects -= ReturnObjects;
    }
    
    private void ReturnObjects()
    {
        foreach (var enemy in _enemyList)
        {
            enemy.GetComponent<Enemy>().Return();
        }
    }
    
    private void CheckNewWave(Enemy enemy)
    {
        _enemyList.Remove(enemy);
        EnemiesLeft--;
        if (EnemiesLeft > 0)
        {
            return;
        }
        if (CurrentWave < 3)
        {
            CurrentWave++;
        }
        else
        {
            CurrentWave = 0;
        }
        _enemyList.Clear();
        StartCoroutine(NewWave());
    }
    
    private IEnumerator NewWave()
    {
        yield return _timeDelay;
        ResetStartPos();
        yield return _timeNewWave;
        SpawnEnemy(CurrentWave);
    }

    private void ResetStartPos()
    {
        _enemyBoard.transform.DOKill();
        _enemyBoard.transform.position = Vector3.zero;
        EnemiesLeft = 20;
    }
    
    public void SpawnEnemy(int spawnteam)
    {
        for (int i = 0; i < 20; i++)
        {
            int groupIndex = i / 5; // Cứ mỗi 5 con sẽ đổi nhóm

            Vector3 spawnPos = (groupIndex % 2 == 0) ? _leftSpawnPoint.position : _rightSpawnPoint.position;

            var enemy = ObjectPooler.Instance.SpawnFromPool("Enemy", spawnPos);
            var enemyScript = enemy.GetComponent<Enemy>();
            switch (spawnteam)
            {
                case 0: 
                    enemyScript.SetType(0);
                    break;
                case 1:
                    int row = i / 5;
                    enemyScript.SetType(row % 2);
                    break;
                case 2:
                    enemyScript.SetType(1);
                    break;
                case 3:
                    enemyScript.SetType(i);
                    break;
            }
            enemy.transform.SetParent(_enemyParent);
            enemy.transform.localScale = _localScale;
            _enemyList.Add(enemyScript);
        }

        StartCoroutine(MoveEnemy());
    }
    
    private IEnumerator MoveEnemy()
    {
        for (int i = 0; i < _enemyList.Count; i++)
        {
            int groupIndex = i / 5;
            Vector3[] worldPath1 = new Vector3[]
            {
                _leftPivot.localPosition,
                _enemyPath[i].transform.localPosition
            };
            
            Vector3[] worldPath2 = new Vector3[]
            {
                _rightPivot.localPosition,
                _enemyPath[i].transform.localPosition
            };
            
            Vector3[] worldPath = (groupIndex % 2 == 0) ? worldPath1 : worldPath2;

            // Di chuyển theo path
            _enemyList[i].transform.DOPath(worldPath, _timeMove, PathType.CatmullRom)
                .SetEase(Ease.InOutSine);

            yield return _wait;
        }

        yield return _timeDelay;
        
        Move();
    }

    private void Move()
    {
        TurnCollider();
        _enemyBoard.transform.DOMoveX(_endPos.position.x, _time * 0.5f)
            .From(0)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => 
            {
                _enemyBoard.transform.DOMoveX(_startPos.position.x, _time)
                    .SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo);
            });
    }
    
    private void TurnCollider()
    {
        foreach (var enemy in _enemyList)
        {
            enemy.ChangeCol(true);
            enemy.StartAttack();
        }
    }
}
