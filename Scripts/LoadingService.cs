using TinyReactive.Fields;
using TinyServices.Windows;

namespace TinyServices.Loading {
    public abstract class LoadingService {
        public Observed<bool> isVisible;
        public InputListener onShow;
        public InputListener onHide;
        
        internal bool isForce;
        
        public static LoadingService instance { get; internal set; }
        
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
    
    public abstract class LoadingService<T> : LoadingService where T : LoadingService<T>, new() {
        public static new T instance { get; private set; }
        
        static LoadingService() {
            instance = new T();
            instance.Init();
            LoadingService.instance = instance;
        }
    }
    
    public abstract class LoadingService<T1, T2> : LoadingService<T1> where T1 : LoadingService<T1, T2>, new() where T2 : LoadingWindow {
        public static new T1 instance { get; private set; }
        
        static LoadingService() {
            instance = new T1();
            instance.Init();
            LoadingService.instance = instance;
        }
        
        protected override void ShowWindow() => WindowsService.Show<T2>();
        
        protected override void HideWindow() => WindowsService.Hide<T2>();
    }
}