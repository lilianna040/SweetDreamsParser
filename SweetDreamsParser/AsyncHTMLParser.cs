using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Dom.Events;
using AngleSharp.Text;

namespace SweetDreamsParser;

public class AsyncHTMLParser
{   
    private string _currentLink;
    public string CurrentLink 
    {
        get { return _currentLink; }
        set { 
                if (string.IsNullOrEmpty(value)) // если новое значение пустое или null
                {
                    throw new ArgumentException("Link not found"); // выбрасываем исключение
                }
                _currentLink = value; 
            }
    }
    public List<string> pageTextContent = new List<string>();

    public Dictionary<int, string> linkCollection = new Dictionary<int, string> 
        {
            {1, "https://datki.net/spokoynoy-nochi/"}, 
            {2, "https://datki.net/spokoynoy-nochi/page/2/"},
            {3, "https://datki.net/spokoynoy-nochi/page/3/"},
            {4, "https://datki.net/spokoynoy-nochi/page/4/"}
        };

    public AsyncHTMLParser(string currentLink = "https://datki.net/spokoynoy-nochi/") 
    {
        this.CurrentLink = currentLink; 
    }

    private async Task<IDocument> ObtainPage() 
    {
        //Use the default configuration for AngleSharp
        var config = Configuration.Default.WithDefaultLoader();

        //Create a new context for evaluating webpages with the given config
        var context = BrowsingContext.New(config);
        return await context.OpenAsync(this.CurrentLink);
    }

    public async Task<int> GetLastPageElement() {
        //Return null if 
        IDocument document = await ObtainPage();
        var lastPageElement = document.QuerySelectorAll(".page-numbers").SkipLast(1).LastOrDefault();
        if (lastPageElement == null)
        {
            return 0;
        }
        return lastPageElement.TextContent.ToInteger(0);
    } 

    public async Task<List<string>> ParsePage(int pageNumber)
    {      
        if (linkCollection.ContainsKey(pageNumber)) // проверяем, есть ли в словаре ключ "pageNumber"
        {
            CurrentLink = linkCollection[pageNumber];
            //Console.WriteLine(CurrentLink);
        }
        else
        {
            Console.WriteLine("Такой страницы не существует, установлена первая страница");
            CurrentLink = linkCollection[1];
        }

        var document = await ObtainPage();

        var p = document.QuerySelectorAll("p").Select(p => p.TextContent.Trim());
        var allParagraphs = p.SkipLast(1);
        var contents = new List<string>();
        foreach (var element in allParagraphs) 
        {
            contents.Add(element);
            //Console.WriteLine(element);
        }
        return contents;
    }

    public async Task<List<string>> ParsePages(int[] pageNumbers)
    {
         var contents = new List<string>();
         foreach (int page in pageNumbers)
         {
            var content = await this.ParsePage(page);
            contents.AddRange(content);
         }
         return contents;
    }

// // init config and context (ne nada)
// IConfiguration? config = Configuration.Default.WithDefaultLoader();
// IBrowsingContext? context = BrowsingContext.New(config);



// // nada: 
// int lastPageNumber = 4; // put ur method GetLastPageNumber or smth
// var url = "https://datki.net/spokoynoy-nochi/"; // ur url


// // List collection for html pages
// List<IDocument> list = new(){await context.OpenAsync(url)};

// // Adding pages
// for (int i = 2; i <= lastPageNumber; i++)
// {
//     IDocument? document = await context.OpenAsync($"{url}page/{i}");
//     list.Add(document);
// }






// FOR GetLastPageNumber METHOD
// var lastPageElement = document.QuerySelectorAll(".page-numbers").SkipLast(1).LastOrDefault();
// foreach (var item in lastPageElement) { Console.WriteLine(item.TextContent); }
// Console.WriteLine(lastPageElement!.TextContent);


}
