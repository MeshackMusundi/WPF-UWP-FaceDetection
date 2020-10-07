using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using FaceDetection.Services.Interfaces;
using Windows.Media.FaceAnalysis;
using BitmapDecoder = Windows.Graphics.Imaging.BitmapDecoder;
using System.Drawing;
using Color = System.Drawing.Color;
using Pen = System.Drawing.Pen;

namespace FaceDetection.Services
{
    public class FaceDetectionService : IFaceDetectionService
    {
        public async Task<IList<DetectedFace>> DetectFaces(Stream fileStream)
        {
            var stream = fileStream.AsRandomAccessStream();
            var bitmapDecoder = await BitmapDecoder.CreateAsync(stream);

            using SoftwareBitmap bitmap = await bitmapDecoder.GetSoftwareBitmapAsync();

            var bmp = FaceDetector.IsBitmapPixelFormatSupported(bitmap.BitmapPixelFormat) 
                ? bitmap : SoftwareBitmap.Convert(bitmap, BitmapPixelFormat.Gray8);

            var faceDetector = await FaceDetector.CreateAsync();
            var detectedFaces = await faceDetector.DetectFacesAsync(bmp);

            return detectedFaces;
        }

        /// <summary>
        /// Creates a bitmap image with detected faces bounded by boxes.
        /// </summary>
        /// <param name="fileStream">The stream of the image file.</param>
        /// <param name="detectedFaces">The collection of faces detected in the image.</param>
        /// <param name="boxColor">The stroke color of the bounding boxes.</param>
        /// <param name="strokeThickness">The thickness of a bounding box's border.</param>
        public Bitmap DetectedFacesBitmap(Stream fileStream, IList<DetectedFace> detectedFaces, 
            Color boxColor, int strokeThickness = 2)
        {
            var bitmap = new Bitmap(fileStream);
            
            using (var graphics = Graphics.FromImage(bitmap))
            {
                using var stroke = new Pen(boxColor, strokeThickness);
                foreach (var face in detectedFaces)
                {
                    BitmapBounds faceBox = face.FaceBox;
                    graphics.DrawRectangle(stroke, (int)faceBox.X, (int)faceBox.Y,
                        (int)faceBox.Width, (int)faceBox.Height);
                }
            }
            return bitmap;
        }
    }
}