
/* Summary
 * Collections is a class which stores each collection is opened or not & sync with Azure
 */
public class Collections
{
    public string ID { get; set; } // playerID
    public int n_collection = 12; // total(possible) collection number
    public string opened = string.Empty; // stores each collection is opened(1) or not(0) and connect them with ',' so that opened variable can one string
}

