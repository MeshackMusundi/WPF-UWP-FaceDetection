using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Media.FaceAnalysis;
using Color = System.Drawing.Color;

namespace FaceDetection.Services.Tests
{
    [TestClass()]
    public class FaceDetectionServiceTests
    {
        private static FaceDetectionService faceDetectionService;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context) 
            => faceDetectionService = new FaceDetectionService();

        [TestMethod()]
        public async Task DetectFacesTest_ImageContainingFaces_FacesDetected()
        {
            var facesImage = Path.Combine(Environment.CurrentDirectory, @"Images\Faces.jpg");
            IList<DetectedFace> detectedFaces;

            await using (FileStream fileStream = File.OpenRead(facesImage))
                detectedFaces = await faceDetectionService.DetectFaces(fileStream);

            Assert.IsNotNull(detectedFaces);
            Assert.IsTrue(detectedFaces.Count == 24);
        }

        [TestMethod()]
        public async Task DetectedFacesBitmapTest_FacesDetected_DetectedFacesBoundedByBoxes()
        {
            var facesImage = Path.Combine(Environment.CurrentDirectory, @"Images\Selfie.jpeg");
            var boxColor = Color.GreenYellow;

            await using FileStream fileStream = File.OpenRead(facesImage);
            var detectedFaces = await faceDetectionService.DetectFaces(fileStream);
            var bitmapImage = faceDetectionService.DetectedFacesBitmap(fileStream, detectedFaces, boxColor);

            foreach (var face in detectedFaces)
            {
                var faceBox = face.FaceBox;
                var pixelColor = bitmapImage.GetPixel((int)faceBox.X, (int)faceBox.Y);
                var isPixelGreen = pixelColor.ToArgb().Equals(boxColor.ToArgb());
                Assert.IsTrue(isPixelGreen);
            }

            var detectedFacesImage = Path.Combine(Environment.CurrentDirectory, @"Images\DetectedFaces.jpg");
            bitmapImage.Save(detectedFacesImage);
        }
    }
}