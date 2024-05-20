namespace NGOAPP;

public class Speaker : BaseViewModel
{
    public string Name { get; set; }
    public string Bio { get; set; }

    // Use ICollection<T> for a flexible collection type (replace T with actual session class)
    public ICollection<Session> Sessions { get; set; } = new List<Session>();
    public List<Media> Images { get; set; } // Assuming a string representing image URLs or paths
    public int EventId { get; set; }
}
