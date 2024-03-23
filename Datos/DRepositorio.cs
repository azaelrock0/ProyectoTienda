using Datos.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class DRepositorio<T> : IRepositorioBase<T> where T : class
    {
        public TiendaContext _DBContext = new TiendaContext();
        public void Actualizar(T entidad)
        {
            _DBContext.ChangeTracker.LazyLoadingEnabled = false;
            _DBContext.Entry(entidad).State = EntityState.Modified;
            _DBContext.SaveChanges();
        }

        public void Agregar(T entidad)
        {
            _DBContext.ChangeTracker.LazyLoadingEnabled = false;
            _DBContext.Set<T>().Add(entidad);
            _DBContext.SaveChanges();
        }

        public void Agregar(List<T> entidades)
        {
            _DBContext.ChangeTracker.LazyLoadingEnabled = false;
            foreach (var entidad in entidades)
            {
                _DBContext.Set<T>().Add(entidad);
            }
            _DBContext.SaveChanges();
        }

        public List<T> Consultar(Expression<Func<T, bool>> predicado)
        {
            _DBContext.ChangeTracker.LazyLoadingEnabled = false;
            return (List<T>)_DBContext.Set<T>().Where(predicado).ToList();
        }

        public List<T> Consultar()
        {
            return (List<T>)_DBContext.Set<T>().ToList();
        }

        public T Consultas(int id)
        {
            _DBContext.ChangeTracker.LazyLoadingEnabled = false;
            return _DBContext.Set<T>().Find(id);
        }

        public T Consultas(Expression<Func<T, bool>> predicado)
        {
            _DBContext.ChangeTracker.LazyLoadingEnabled = false;
            return _DBContext.Set<T>().FirstOrDefault<T>(predicado);
        }

        public void Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public void Eliminar(T entidad)
        {
            _DBContext.ChangeTracker.LazyLoadingEnabled = false;
            _DBContext.Set<T>().Remove(entidad);
            _DBContext.SaveChanges();
        }

        public void Eliminar(List<T> entidades)
        {
            _DBContext.ChangeTracker.LazyLoadingEnabled = false;
            foreach (var entidad in entidades)
            {
                _DBContext.Set<T>().Remove(entidad);
            }
            _DBContext.SaveChanges();
        }

        public void Eliminar(Expression<Func<T, bool>> predicado)
        {
            _DBContext.ChangeTracker.LazyLoadingEnabled = false;
            var entities = _DBContext.Set<T>().Where(predicado).ToList();
            entities.ForEach(x => _DBContext.Entry(x).State = EntityState.Deleted);
            _DBContext.SaveChanges();
        }
    }
}
