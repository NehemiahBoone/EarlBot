using System.Threading.Tasks;

namespace EarlBot
{
    class Program
    {
        public static async Task Main(string[] args)
            => await Startup.RunAsync(args);
    }
}
