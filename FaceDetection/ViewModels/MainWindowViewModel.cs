using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using FaceDetection.Commands;
using FaceDetection.Services.Interfaces;

namespace FaceDetection.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IDialogService dialogService;
        private readonly IFaceDetectionService faceDetectionService;

        public MainWindowViewModel(IDialogService dialogSvc, IFaceDetectionService faceDetectionSvc)
        {
            dialogService = dialogSvc;
            faceDetectionService = faceDetectionSvc;
        }

        private string _selectedImage;
        public string SelectedImage
        {
            get => _selectedImage;
            set
            {
                _selectedImage = value;
                OnPropertyChanged();
            }
        }

        #region Select Image Command
        private RelayCommand _selectImageCommand;
        public RelayCommand SelectImageCommand =>
            _selectImageCommand ??= new RelayCommand(_ => SelectImage());

        private void SelectImage()
        {
            var image = dialogService.PickFile("Select Image",
                "Image (*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png");

            if (string.IsNullOrWhiteSpace(image)) return;

            SelectedImage = image;
        }
        #endregion

        private Bitmap _facesBitmap;
        public Bitmap FacesBitmap
        {
            get => _facesBitmap;
            set
            {
                _facesBitmap = value;
                OnPropertyChanged();
            }
        }

        #region Detect faces Command
        private RelayCommandAsync _detectFacesCommand;
        public RelayCommandAsync DetectFacesCommand =>
            _detectFacesCommand ??= new RelayCommandAsync(DetectFaces, _ => CanDetectFaces());

        private async Task DetectFaces()
        {
            await using FileStream fileStream = File.OpenRead(_selectedImage);
            var faces = await faceDetectionService.DetectFaces(fileStream);
            FacesBitmap = faceDetectionService.DetectedFacesBitmap(fileStream, faces, Color.GreenYellow);
            SelectedImage = null;
        }

        private bool CanDetectFaces() => !string.IsNullOrWhiteSpace(SelectedImage);
        #endregion
    }
}
