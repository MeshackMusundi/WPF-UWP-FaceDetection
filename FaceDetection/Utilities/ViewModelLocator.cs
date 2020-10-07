using FaceDetection.Services;
using FaceDetection.Services.Interfaces;
using FaceDetection.ViewModels;
using Unity;

namespace FaceDetection.Utilities
{
    public class ViewModelLocator
    {
        private readonly UnityContainer container;

        public ViewModelLocator()
        {
            container = new UnityContainer();
            container.RegisterType<IDialogService, DialogService>();
            container.RegisterType<IFaceDetectionService, FaceDetectionService>();
        }

        public MainWindowViewModel MainWindowVM => container.Resolve<MainWindowViewModel>();
    }
}
