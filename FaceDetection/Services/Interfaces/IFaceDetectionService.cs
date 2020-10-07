using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Windows.Media.FaceAnalysis;
using Color = System.Drawing.Color;

namespace FaceDetection.Services.Interfaces
{
    public interface IFaceDetectionService
    {
        Task<IList<DetectedFace>> DetectFaces(Stream fileStream);

        Bitmap DetectedFacesBitmap(Stream fileStream, IList<DetectedFace> detectedFaces,
            Color boxColor, int strokeThickness = 2);
    }
}
