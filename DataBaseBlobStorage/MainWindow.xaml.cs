using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace DataBaseBlobStorage
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnConvert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string PathFile = TxPathConvert.Text;
                string zipPath = @$"{Path.GetDirectoryName(PathFile)}\{Path.GetFileNameWithoutExtension(PathFile)}.zip";

                if (File.Exists(zipPath)) File.Delete(zipPath);

                using (ZipArchive zip = ZipFile.Open(zipPath, ZipArchiveMode.Create))
                {
                    zip.CreateEntryFromFile(PathFile, System.IO.Path.GetFileName(PathFile));
                    if (File.Exists(zipPath)) MessageBox.Show("el archivo se convirtio a zip exitosamente");

                }

            }
            catch (IOException w)
            {
                MessageBox.Show("error en IO:" + w);
            }
            catch (Exception w)
            {
                MessageBox.Show("error al convertir:" + w);
            }
        }

        private async void BtnUpload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string pathUpload = TxPathUpload.Text;

                string name = Path.GetFileName(pathUpload);
                string paths = Path.GetFullPath(pathUpload);

                MessageBox.Show(paths);


                string connection = "DefaultEndpointsProtocol=https;AccountName=almacenamientotemp;AccountKey=t3eiYMeR3DZKWA2ONFaA0xBbxBqVgGjl+FAhIud0ruC4majBB3c+NGWqoyM4PTEPNIr1AfJSApRFOM5nTE/05A==;EndpointSuffix=core.windows.net";


                BlobServiceClient blobServiceClient = new BlobServiceClient(connection);
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("desdecodigo");
                BlobClient blobClient = containerClient.GetBlobClient(name);
                using (var fileStream = System.IO.File.OpenRead(paths))
                {
                    await blobClient.UploadAsync(fileStream, true);
                }

                MessageBox.Show("se subio el archivo exitosamente");


            }
            catch (Exception w)
            {
                MessageBox.Show("error al subir el archivo:" + w);
            }
        }




    }
}

