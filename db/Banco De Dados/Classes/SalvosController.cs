 using db.Banco_De_Dados.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pontos_de_Domino_UWP.db.Banco_De_Dados.Classes
{
    class SalvosController : IRepositorioTXT<Salvos>
    {
        _dbConection _db;

        public SalvosController()
        {
            _db = new _dbConection();
        }

        public bool Apagar(Salvos txt)
        {
            if (txt.ID > 0)
            {
                _db._conexao.Delete(txt);
                return true;
            }
            else
            {
                return false;
            }
        }

        public Salvos inpID(Salvos ID)
        {
            _db._conexao.Table<Salvos>().Where(i => i.ID > 0);
            return null;
        }

        public List<Salvos> Lista()
        {
            return _db._conexao.Table<Salvos>().ToList();
        }

        public bool Salvar(Salvos txt)
        {
            _db._conexao.Insert(txt);
            return true;
        }
    }
}
