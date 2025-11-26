// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using TinyReactive.Fields;
using TinyServices.Windows;

namespace TinyServices.Loading {
    public abstract class BaseLoadingService {
        public Observed<bool> isVisible;
        public InputListener onShow;
        public InputListener onHide;
        
        internal bool isForce;
        
        public static BaseLoadingService instance { get; internal set; }
        
        protected virtual void Init() {
            isVisible = new Observed<bool>();
            onShow = new InputListener();
            onHide = new InputListener();
        }
        
        public void Show() {
            if (isVisible.value) {
                return;
            }
            
            isForce = false;
            isVisible.Set(true);
            ShowWindow();
        }
        
        public void ShowForce() {
            if (isVisible.value) {
                return;
            }
            
            isForce = true;
            isVisible.Set(true);
            ShowWindow();
        }
        
        public void Hide() {
            if (isVisible.value == false) {
                return;
            }
            
            isForce = false;
            isVisible.Set(false);
            HideWindow();
        }
        
        public void HideForce() {
            if (isVisible.value == false) {
                return;
            }
            
            isForce = true;
            isVisible.Set(false);
            HideWindow();
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