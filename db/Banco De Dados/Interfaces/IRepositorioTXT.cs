using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db.Banco_De_Dados.Interfaces
{
    public interface IRepositorioTXT <T> where T : class
    {
        T inpID(T ID);
        bool Salvar(T txt);
        bool Apagar(T txt);
        List<T> Lista();
    }
}
