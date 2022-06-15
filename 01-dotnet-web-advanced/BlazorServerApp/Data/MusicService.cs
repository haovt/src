using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp.Data
{
    public class MusicService
    {
        private Song[] songs;

        public MusicService()
        {

            songs = new[]
            {
                new Song(1, 1, "Black Light Syndrome", 150),
                new Song(2, 1, "A Copland Celebration, Vol. I", 50),
                new Song(3, 2, "A Lively Mind", 250),
                new Song(4, 2, "A Matter of Life and Death", 70),
                new Song(5, 1, "A Winter Symphony", 17),
                new Song(6, 1, "Afrociberdelia", 70),
                new Song(7, 2, "Alcohol Fueled Brewtality Live! [Disc 1]", 60),
                new Song(8, 2, "Bartok: Violin & Viola Concertos", 120),
                new Song(9, 2, "Brave New World", 180),
                new Song(10, 2, "Dark Side of the Moon", 149)
            };
        }

        public Task<Song[]> GetSongs()
        {
            return Task.FromResult(songs);
        }

        public Task<Song> GetSongById(int id)
        {
            return Task.FromResult(songs.FirstOrDefault(x => x.Id == id));
        }
    }
}
