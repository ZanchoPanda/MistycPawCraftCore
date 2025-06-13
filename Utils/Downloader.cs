using MistycPawCraftCore.Properties;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace MistycPawCraftCore.Utils
{
    public class Downloader
    {

        public static async Task DownloadAsync(string Uri, string Destiny = "", string Namefile = "")
        {

            if (string.IsNullOrWhiteSpace(Destiny))
            {
                Destiny = Settings.Default.DownloadFolder;
                if (string.IsNullOrWhiteSpace(Destiny))
                {
                    return;
                }
            }


            using HttpClient cliente = new HttpClient();
            var bytes = await cliente.GetByteArrayAsync(Uri);

            string extension = Path.GetExtension(new Uri(Uri).AbsolutePath);

            string FinalName = string.IsNullOrWhiteSpace(Namefile) ? Path.GetFileName(Uri) : Namefile + extension;

            string finalroute = Path.Combine(Destiny, FinalName);
            Directory.CreateDirectory(Path.GetDirectoryName(finalroute));

            await File.WriteAllBytesAsync(finalroute, bytes);

            Console.WriteLine("Imagen descargada en: " + Destiny);
        }


    }
}
