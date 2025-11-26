// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using TinyServices.Windows;

namespace TinyServices.Loading {
    public abstract class BaseLoadingWindow : WindowBehavior {
        public override void Show() {
            base.Show();
            ShowComplete();
        }
        
        public virtual void ShowForce() {
            gameObject.SetActive(true);
            ShowComplete();
        }
        
        public override void Hide() {
            base.Hide();
            HideComplete();
        }
        
        public virtual void HideForce() {
            gameObject.SetActive(false);
            HideComplete();
        }
        
        protected virtual void ShowComplete() => BaseLoadingService.instance.OnShowComplete();
        
        protected virtual void HideComplete() => BaseLoadingService.instance.OnHideComplete();
        
        internal override void ShowInternal() {
            isVisible = true;
            
            if (BaseLoadingService.instance.isForce) {
                ShowForce();
            } else {
                Show();
            }
        }
        
        internal override void HideInternal() {
            isVisible = false;
            
            if (BaseLoadingService.instance.isForce) {
                HideForce();
            } else {
                Hide();
            }
        }
    }
}