using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;          //used to parse the JSON data Returned by server
using System.Net.Http;          //used to fetch the data from server
using System.IO;

namespace ImageGalleryDemo
{
    class DataFetcher
    {
        async Task<string> GetDatafromService(string searchstring)
        {
            string readText = null;
            try
            {
                var azure = @"https://imagefetcher20200529182038.azurewebsites.net";
                string url = azure + @"/api/fetch_images?query=" + searchstring + "&max_count=5";
                using(HttpClient c = new HttpClient())          //using HttpClient class to fetch the JSON data from server 
                {
                    readText = await c.GetStringAsync(url);     //returning the response as a string to readText
                }
            }
            catch
            {
                readText = File.ReadAllText(@"Data/sampleData.json");
            }
            return readText;
        }

        public async Task<List<ImageItem>> GetImageData(string search)   //this method is for parsing the JSON data
        {
            string data = await GetDatafromService(search);
            return JsonConvert.DeserializeObject<List<ImageItem>>(data);
        }

    }
}
