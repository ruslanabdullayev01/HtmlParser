using HtmlAgilityPack;
using Newtonsoft.Json;

string url = "https://api.carfax.shop/report/vieww?hash=63797377674e51636b54565250307669486f5467424c413359514d613868695a59413d3d";

using (HttpClient httpClient = new HttpClient())
{
    HttpResponseMessage response = await httpClient.GetAsync(url);

    if (response.IsSuccessStatusCode)
    {
        string htmlContent = await response.Content.ReadAsStringAsync();

        HtmlDocument htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(htmlContent);


        var scriptNode = htmlDocument.DocumentNode.SelectSingleNode("//script[contains(., '__INITIAL__DATA__')]");
        if (scriptNode != null)
        {
            string scriptContent = scriptNode.InnerText;
            int start = scriptContent.IndexOf("{");
            int end = scriptContent.LastIndexOf("}");
            string jsonContent = scriptContent.Substring(start, end - start + 1);

            dynamic dataObject = JsonConvert.DeserializeObject(jsonContent);

            string yearMakeModel = dataObject.vhr.headerSection.vehicleInformationSection.yearMakeModel;

            Console.WriteLine(yearMakeModel);
        }
        else
        {
            Console.WriteLine("Not Found!");
        }
    }
    else
    {
        Console.WriteLine("ERROR");
    }
}