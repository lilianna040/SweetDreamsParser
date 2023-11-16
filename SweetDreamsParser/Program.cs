using SweetDreamsParser;

AsyncHTMLParser page = new AsyncHTMLParser();

foreach (var item in await page.ParsePage(1))
{
    Console.WriteLine(item);
}

foreach (var item in await page.ParsePages([2,4,3]))
{
    Console.WriteLine(item);
}


Console.WriteLine((await page.ParsePage(1)).Count);
Console.WriteLine((await page.ParsePages([2,4,3])).Count);
