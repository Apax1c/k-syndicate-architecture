﻿using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Input;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private const string Initial = "Initial";
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _allServices;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices services)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _allServices = services;

            RegisterServices();
        }

        public void Enter() => 
            _sceneLoader.Load(Initial, onLoaded: EnterLoadLevel);

        public void Exit()
        {

        }

        private void EnterLoadLevel() => 
            _stateMachine.Enter<LoadProgressState>();

        private void RegisterServices()
        {
            _allServices.RegisterSingle<IInputService>(InputService());
            _allServices.RegisterSingle<IAssets>(new AssetProvider());
            _allServices.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
            _allServices.RegisterSingle<IGameFactory>(new GameFactory(_allServices.Single<IAssets>()));
            _allServices.RegisterSingle<ISaveLoadService>(new SaveLoadService(_allServices.Single<IPersistentProgressService>(), _allServices.Single<IGameFactory>()));
        }

        private static IInputService InputService()
        {
            if (Application.isEditor)
                return new StandaloneInputSerice();
            else
                return new MobileInputService();
        }
    }
}