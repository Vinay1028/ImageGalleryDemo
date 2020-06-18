using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using C1.Win.C1Tile;

namespace ImageGalleryDemo
{
    public partial class ImageGallery : Form
    {
        DataFetcher datafetch = new DataFetcher();
        List<ImageItem> imageList;
        int checkedItems = 0;
        public ImageGallery()
        {
            InitializeComponent();
        }

        private async void _search_Click(object sender, EventArgs e)          //search button event
        {
            statusStrip1.Visible = true;
            imageList = await datafetch.GetImageData(_searchBox.Text);
            AddTiles(imageList);
            statusStrip1.Visible = false;
        }

        private void AddTiles(List<ImageItem> imageList)
        {
            _imageTileControl.Groups[0].Tiles.Clear();

            foreach (var imageitem in imageList)
            {
                Tile tile = new Tile();
                tile.HorizontalSize = 2;

                _imageTileControl.Groups[0].Tiles.Add(tile);

                Image img = Image.FromStream(new MemoryStream(imageitem.Base64));

                Template t1 = new Template();
                ImageElement ie = new ImageElement();
                ie.ImageLayout = ForeImageLayout.Stretch;
                tile.Template = t1;
                tile.Image = img;
            }
        }

        private void OnTileChecked(Object sender, TileEventArgs e)
        {
            checkedItems++;
            _exportImage.Visible = true;
        }

        private void OnTileUnchecked(Object sender, TileEventArgs e )
        {
            checkedItems--;
            _exportImage.Visible = checkedItems > 0;
        }

        private void _exportImage_Click(object sender, EventArgs e)
        {
            List<Image> images = new List<Image>();
            foreach(Tile tile in _imageTileControl.Groups[0].Tiles)
            {
                if(tile.Checked)
                {
                    images.Add(tile.Image);
                }
            }
            ConvertToPdf(images);
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.DefaultExt = "pdf";
            saveFile.Filter = "PDF files (*.pdf)|.pdf*";
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                imagePdfDocument.Save(saveFile.FileName);
            }
        }

        private void ConvertToPdf(List<Image> images)
        {
            RectangleF rect = imagePdfDocument.PageRectangle;
            bool firstPage = true;
            foreach(var selectimg in images)
            {
                if(!firstPage)
                {
                    imagePdfDocument.NewPage();
                }
                firstPage = false;

                rect.Inflate(-72, -72);
                imagePdfDocument.DrawImage(selectimg, rect);
            }
        }

        private void _exportImage_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
