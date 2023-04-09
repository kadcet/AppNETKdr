using AppNETKdr.Domain.Entities.Base;
using AppNETKdr.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AppNETKdr.Infrastructure.IOToTXT
{
    public class TextFileRepository<T> : IRepository<T> where T : BaseEntity
    {
        #region Dosya Adı
        private static string FileName
        {
            get
            {
                return typeof(T).FullName.Replace(".", "") + "txt";
            }
        }
        #endregion

        #region Liste
        private static List<T> list = new List<T>();
        #endregion

        #region Dosyadan Okuma Metodu
        private static void LoadListFromFile()
        {
            if (!File.Exists(FileName))
            {
                list = new List<T>();
                return;
            }

            var json = File.ReadAllText(FileName);
            list = JsonSerializer.Deserialize<List<T>>(json);

        }
        #endregion

        #region Dosyaya Yazma Metodu
        private static void WriteListToTxt()
        {
            var jsonText = JsonSerializer.Serialize(list);
            File.WriteAllText(FileName, jsonText);
        }
        #endregion

        #region static Yapıcı Metod Sadece 1 Kere Çalışıcak
        static TextFileRepository()
        {
            LoadListFromFile();
        }
        #endregion

        public T Add(T entity)
        {
            LoadListFromFile();
            list.Add(entity);
            WriteListToTxt();
            return entity;
        }

        public T GetById(int id)
        {
            LoadListFromFile();
            var entity = list.FirstOrDefault(x => x.Id == id);
            return entity;

        }

        public ICollection<T> GetList(Func<T, bool> expression = null)
        {
            LoadListFromFile();
            //expre yoksa listeyi ?(null)dön :değil ise bunu dön
            return expression == null ? list : list.Where(expression).ToList();
        }

        public bool Remove(int id)
        {
            LoadListFromFile();
            var deletedEntity = list.FirstOrDefault(x => x.Id == id);
            if (deletedEntity != null)
            {
                list.Remove(deletedEntity);
                WriteListToTxt();
                return true;
            }
            return false;
        }

        public T Update(int id, T entity)
        {
            if (id != entity.Id)
                throw new ArgumentException("Id değerleri uyuşmuyor");

            LoadListFromFile();
            var updated = list.FirstOrDefault(x => x.Id == id);
            if (updated == null)
                throw new ArgumentException("Güncellemek istenilen varlık bulnamadı");

            list.Remove(updated);
            list.Add(entity);
            WriteListToTxt();
            return entity;

        }
    }
}
