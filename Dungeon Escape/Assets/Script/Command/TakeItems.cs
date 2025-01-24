public class TakeItems : Command 
{
    // Start is called before the first frame update
    public override void Execute(){}
    
}
public struct takeItemParams
{
    public string Item;
    public takeItemParams(string item)
    {
        Item = item;
    }
}
