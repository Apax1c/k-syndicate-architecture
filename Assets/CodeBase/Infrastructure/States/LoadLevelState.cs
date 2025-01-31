﻿using System.Collections.Generic;
using CodeBase.CameraLogic;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using CodeBase.UI;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Factory;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.States
{
  public class LoadLevelState : IPayloadedState<string>
  {
    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _loadingCurtain;
    private readonly IGameFactory _gameFactory;
    private readonly IPersistentProgressService _progressService;
    private readonly IStaticDataService _staticData;
    private readonly IUIFactory _uiFactory;

    public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain, IGameFactory gameFactory, IPersistentProgressService progressService, IStaticDataService staticDataService, IUIFactory uiFactory)
    {
      _stateMachine = gameStateMachine;
      _sceneLoader = sceneLoader;
      _loadingCurtain = loadingCurtain;
      _gameFactory = gameFactory;
      _progressService = progressService;
      _staticData = staticDataService;
      _uiFactory = uiFactory;
    }

    public void Enter(string sceneName)
    {
      _loadingCurtain.Show();
      _gameFactory.Cleanup();
      _sceneLoader.Load(sceneName, OnLoaded);
    }

    public void Exit() =>
      _loadingCurtain.Hide();

    private void OnLoaded()
    {
      InitUIRoot();
      InitGameWorld();
      InformProgressReaders();

      _stateMachine.Enter<GameLoopState>();
    }

    private void InitUIRoot() => 
      _uiFactory.CreateUIRoot();

    private void InformProgressReaders()
    {
      foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
        progressReader.LoadProgress(_progressService.Progress);
    }

    private void InitGameWorld()
    {
      var levelData = LevelStaticData();
      
      InitSpawners(levelData);
      InitLootPieces();
      GameObject hero = InitHero(levelData);
      InitHud(hero);
      CameraFollow(hero);
    }

    private GameObject InitHero(LevelStaticData levelData) => 
      _gameFactory.CreateHero(levelData.InitialHeroPosition);

    private void InitSpawners(LevelStaticData levelData)
    {
      foreach (EnemySpawnerStaticData spawnerData in levelData.EnemySpawners)
        _gameFactory.CreateSpawner(spawnerData.Id, spawnerData.Position, spawnerData.MonsterTypeId);
    }

    private void InitLootPieces()
    {
      foreach (KeyValuePair<string, LootPieceData> item in _progressService.Progress.WorldData.LootData.LootPiecesOnScene.Dictionary)
      {
        LootPiece lootPiece = _gameFactory.CreateLoot();
        lootPiece.GetComponent<UniqueId>().Id = item.Key;
        lootPiece.Initialize(item.Value.Loot);
        lootPiece.transform.position = item.Value.Position.AsUnityVector();
      }
    }

    private void InitHud(GameObject hero)
    {
      GameObject hud = _gameFactory.CreateHud();
      
      hud.GetComponentInChildren<ActorUI>().Construct(hero.GetComponent<HeroHealth>());
    }

    private LevelStaticData LevelStaticData() => 
      _staticData.ForLevel(SceneManager.GetActiveScene().name);

    private void CameraFollow(GameObject hero) =>
      Camera.main.GetComponent<CameraFollow>().Follow(hero);
  }
}