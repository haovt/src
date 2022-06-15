namespace BlazorServerApp.Data
{
    public class Song
    {
        public Song(int id, int albumId, string name, decimal price)
        {
            Id = id;
            AlbumId = albumId;
            Name = name;
            Price = price;
        }

        public int Id { get; set; }
        public int AlbumId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
