using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public interface IRepositorioBase<T> where T : class
    {
        List<T> Consultar();
        List<T> Consultar(Expression<Func<T, bool>> predicado);
        T Consultas(int id);
        T Consultas(Expression<Func<T, bool>> predicado);
        void Agregar(T entidad);
        void Agregar(List<T> entidades);
        void Actualizar(T entidad);
        void Eliminar(int id);
        void Eliminar(T Entidad);
        void Eliminar(List<T> Entidades);
        void Eliminar(Expression<Func<T, bool>> predicado);
    }
}
