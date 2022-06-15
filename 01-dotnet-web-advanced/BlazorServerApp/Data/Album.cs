using System.Collections.Generic;

namespace BlazorServerApp.Data
{
    public class Album
    {
        public Album(int id, string title)
        {
            AlbumId = id;
            Title = title;
        }

        public int AlbumId { get; set; }
        public string Title { get; set; }

        public IList<Song> Songs { get; set; }
    }
}
