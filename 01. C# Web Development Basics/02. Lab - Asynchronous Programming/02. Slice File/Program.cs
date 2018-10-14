namespace _02.Slice_File
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using static System.Console;

    internal static class Program
    {
        private static void Main()
        {
            string videoPath = ReadLine();
            string destination = ReadLine();
            int pieces = int.Parse(ReadLine());

            SliceAsync(videoPath, destination, pieces);

            WriteLine("Anything else?");

            while (true)
            {
                WriteLine(ReadLine());
            }
        }

        private static void SliceAsync(string sourceFile, string destinationPath, int parts)
        {
            Task.Run(() => Slice(sourceFile, destinationPath, parts));
        }

        private static void Slice(string sourceFile, string destinationPath, int parts)
        {
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            using (FileStream source = new FileStream(sourceFile, FileMode.Open))
            {
                FileInfo fileInfo = new FileInfo(sourceFile);

                long partLength = (source.Length / parts) + 1;
                long currentByte = 0;

                for (int currentPart = 1; currentPart <= parts; currentPart++)
                {
                    string filePath = $"{destinationPath}/Part-{currentPart}{fileInfo.Extension}";

                    using (FileStream destination = new FileStream(filePath, FileMode.Create))
                    {
                        byte[] buffer = new byte[4096];
                        while (currentByte <= partLength * currentPart)
                        {
                            int readBytesCount = source.Read(buffer, 0, buffer.Length);
                            if (readBytesCount == 0)
                            {
                                break;
                            }

                            destination.Write(buffer, 0, readBytesCount);
                            currentByte += readBytesCount;
                        }
                    }
                }

                WriteLine("Slice complete");
            }
        }
    }
}