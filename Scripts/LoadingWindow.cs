using TinyServices.Windows;

namespace TinyServices.Loading {
    public abstract class LoadingWindow : WindowBehavior {
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
        
        protected void ShowComplete() => LoadingService.instance.onShow.Send();
        
        protected void HideComplete() => LoadingService.instance.onHide.Send();
        
        internal override void ShowInternal() {
            isVisible = true;
            
            if (LoadingService.instance.isForce) {
                ShowForce();
            } else {
                Show();
            }
        }
        
        internal override void HideInternal() {
            isVisible = false;
            
            if (LoadingService.instance.isForce) {
                HideForce();
            } else {
                Hide();
            }
        }
    }
}