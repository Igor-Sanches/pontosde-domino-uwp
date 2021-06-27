using System; 
using SQLite.Net.Platform.WinRT;
using SQLite.Net;
using System.IO; 
using Pontos_de_Domino_UWP.db.Banco_De_Dados.Classes;
using Windows.Storage;
using System.Threading.Tasks;

namespace Pontos_de_Domino_UWP.db.Banco_De_Dados
{
    public class _dbConection : IDisposable
    {
        public SQLiteConnection _conexao; 
        public _dbConection()
        {
            //if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            _conexao = new SQLiteConnection(new SQLitePlatformWinRT(), Path.Combine(ApplicationData.Current.LocalFolder.Path, "Faby.db"));
            _conexao.CreateTable<Salvos>(); 

        }

        private async void Criar_db()
        {
        }

        private async Task<string> dir()
        {
            var pasta = await ApplicationData.Current.LocalFolder.CreateFolderAsync("Pontos de Dominó", CreationCollisionOption.OpenIfExists);
            var subPasta = await pasta.CreateFolderAsync("Igor Sanches", CreationCollisionOption.OpenIfExists);
            return subPasta.Path;
        }

        public void Dispose()
        {
            _conexao.Dispose();
        }
    }
}
