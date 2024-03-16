using System;
using System.IO;
using System.Windows.Media;
using HelixToolkit.Wpf;

namespace VerasDotNetSample
{
    public class ImageCapture
    {
        public string baseImage { get; set; } = "";
        private HelixViewport3D HelixView;

        public ImageCapture(HelixViewport3D helixView)
        {
            this.HelixView = helixView;
        }

        public void CapturePreviewImageBase64String()
        {
            string filePath = GetOrCreateImagePath();
            Viewport3DHelper.Export(HelixView.Viewport, filePath, new SolidColorBrush(Color.FromRgb(255,255,255)));
            baseImage = ReadBase64ImageFromFile(filePath);
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
