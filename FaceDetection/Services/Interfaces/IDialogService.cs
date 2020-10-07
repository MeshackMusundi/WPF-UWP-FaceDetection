using System.IO;

namespace FaceDetection.Services.Interfaces
{
    public interface IDialogService
    {
        string PickFile(string caption, string filter = "All files (*.*)|*.*");
        Stream OpenFile(string caption, string filter = "All files (*.*)|*.*");
    }
}
