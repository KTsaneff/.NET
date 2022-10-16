namespace ProductShop.Common
{
    using System.IO;
    public static class PathInitializer
    {
        private static string filePath = "";
        public static string CombineImportPath(string fileName, string directory)
        {
            filePath = Path.Combine(directory, "../../../Datasets/", fileName);
            return filePath;
        }

        public static string CombineExportPath(string fileName, string directory)
        {
            filePath = Path.Combine(directory, "../../../Datasets/Results/", fileName);

            return filePath;
        }
    }
}
