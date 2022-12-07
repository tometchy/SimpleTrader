namespace SimpleTrader.Bet;

public static class ListPublisher
{
    private static readonly string ListFilePath = "/var/simple-trader/list.html";

    private const string ADD_HERE_COMMENT = "<!-- ADD HERE -->";

    public static void Publish(string newItem)
    {
        if(!File.Exists(ListFilePath))
            File.WriteAllText(ListFilePath, @$"
<!DOCTYPE html>
<html>
<body>
NOFOLLOW!!!!!!!!!!!!!!!!!!!!

<ul>
{ADD_HERE_COMMENT}
<!--  <li>Coffee</li> -->
</ul>

</body>
</html>
");
        
        string text = File.ReadAllText(ListFilePath);
        text = text.Replace(ADD_HERE_COMMENT, $"{ADD_HERE_COMMENT}\n<li>{newItem}</li>");
        File.WriteAllText(ListFilePath, text);        
    }
}