using System;
using System.IO;
using System.Windows.Media;
using HelixToolkit.Wpf;

namespace VerasDotNetSample
{
    /// <summary>
    /// Class handles getting an image from the sample design app, and storing the image as a base64 string
    /// </summary>
    public class ImageCapture
    {
        private HelixViewport3D HelixView;

        public ImageCapture(HelixViewport3D helixView)
        {
            this.HelixView = helixView;
        }

        public string GetPreviewImageBase64String()
        {
            string filePath = GetOrCreateImagePath();
            Viewport3DHelper.Export(HelixView.Viewport, filePath, new SolidColorBrush(Color.FromRgb(255,255,255)));
            string baseImage = ReadBase64ImageFromFile(filePath);
            return baseImage;
        }

        public string GetPreviewImageBase64StringWithLight()
        {
            HelixView.IsHeadLightEnabled = true;
            string filePath = GetOrCreateImagePath();
            Viewport3DHelper.Export(HelixView.Viewport, filePath, new SolidColorBrush(Color.FromRgb(255, 255, 255)));
            string baseImage = ReadBase64ImageFromFile(filePath);
            HelixView.IsHeadLightEnabled = false;

            return baseImage;
        }

        private string GetOrCreateImagePath()
        {
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "VerasDotNetSample\\cache");
            Directory.CreateDirectory(folderPath);
            string fileName = "baseImage";
            string filepath = Path.Combine(folderPath, fileName);
            return Path.ChangeExtension(filepath, "png");
        }

        private string ReadBase64ImageFromFile(string filePath)
        {
            // read image file as binary.
            byte[] imageArray = File.ReadAllBytes(filePath);

            // convert binary to 64-bit string.
            return Convert.ToBase64String(imageArray);
        }
    }
}
