// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Threading;
using Cysharp.Threading.Tasks;
using TinyReactive;
using TinyReactive.Fields;
using TinyServices.Windows;

namespace TinyServices.Loading {
    public abstract class BaseLoadingService {
        public bool isVisible { get; private set; }
        
        internal bool isForce;
        
        protected virtual bool _isVisible { get; }
        
        private InputListener _onShow;
        private InputListener _onHide;
        private UnloadPool _unloadShow;
        private UnloadPool _unloadHide;
        private bool _onShowComplete;
        private bool _onHideComplete;
        
        public static BaseLoadingService instance { get; internal set; }
        
        protected virtual void Init() {
            isVisible = _isVisible;
            _onShow = new InputListener();
            _onHide = new InputListener();
            _unloadShow = new UnloadPool();
            _unloadHide = new UnloadPool();
        }
        
        public void AddListenerShow(ActionListener listener) => AddListenerShow(listener, _unloadShow);
        
        public void AddListenerShow(ActionListener listener, UnloadPool unload) => _onShow.AddListener(listener, unload);
        
        public void AddListenerHide(ActionListener listener) => AddListenerHide(listener, _unloadHide);
        
        public void AddListenerHide(ActionListener listener, UnloadPool unload) => _onHide.AddListener(listener, unload);
        
        public void ShowForce() {
            if (isVisible == false) {
                isForce = true;
                isVisible = true;
                ShowWindow();
            }
        }
        
        public void Show() {
            if (isVisible == false) {
                isForce = false;
                isVisible = true;
                ShowWindow();
            }
        }
        
        public void Show(ActionListener onComplete) {
            if (isVisible == false) {
                _onShow.AddListener(onComplete, _unloadShow);
                isForce = false;
                isVisible = true;
                ShowWindow();
            }
        }
        
        public async UniTask ShowAsync(CancellationToken cancellation) {
            if (isVisible == false) {
                isForce = false;
                isVisible = true;
                _onShowComplete = false;
                ShowWindow();
                await UniTask.WaitUntil(() => _onShowComplete, PlayerLoopTiming.Update, cancellation);
            }
        }
        
        public void HideForce() {
            if (isVisible) {
                isForce = true;
                isVisible = false;
                HideWindow();
            }
        }
        
        public void Hide() {
            if (isVisible) {
                isForce = false;
                isVisible = false;
                HideWindow();
            }
        }
        
        public void Hide(ActionListener onComplete) {
            if (isVisible) {
                _onHide.AddListener(onComplete, _unloadHide);
                isForce = false;
                isVisible = false;
                HideWindow();
            }
        }
        
        public async UniTask HideAsync(CancellationToken cancellation) {
            if (isVisible) {
                isForce = false;
                isVisible = false;
                _onHideComplete = false;
                HideWindow();
                await UniTask.WaitUntil(() => _onHideComplete, PlayerLoopTiming.Update, cancellation);
            }
        }
        
        internal void OnShowComplete() {
            _onShow.Send();
            _unloadShow.Unload();
            _onShowComplete = true;
        }
        
        internal void OnHideComplete() {
            _onHide.Send();
            _unloadHide.Unload();
            _onHideComplete = true;
        }
        
        protected abstract void ShowWindow();
        
        protected abstract void HideWindow();
    }
    
    public abstract class BaseLoadingService<T> : BaseLoadingService where T : BaseLoadingService<T>, new() {
        public static new T instance { get; private set; }
        
        static BaseLoadingService() {
            instance = new T();
            instance.Init();
            BaseLoadingService.instance = instance;
        }
    }
    
    public abstract class BaseLoadingService<T1, T2> : BaseLoadingService<T1> where T1 : BaseLoadingService<T1, T2>, new() where T2 : BaseLoadingWindow {
        public static new T1 instance { get; private set; }
        
        static BaseLoadingService() {
            instance = new T1();
            instance.Init();
            BaseLoadingService.instance = instance;
        }
        
        protected override void ShowWindow() => WindowsService.Show<T2>();
        
        protected override void HideWindow() => WindowsService.Hide<T2>();
    }
}