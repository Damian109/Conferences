using System.ComponentModel.DataAnnotations.Schema;

namespace Conferences.bdWork
{
    //Класс для хранения строки из таблицы, в которой группам необходимы различные услуги
    [Table("GroupService")]
    public class GroupService
    {
        //Группа, внешний ключ
        private int _idGroup;

        public int IdGroup
        {
            get { return _idGroup; }
            set { _idGroup = value; }
        }

        //Услуга, внешний ключ
        private int _idService;

        public int IdService
        {
            get { return _idService; }
            set { _idService = value; }
        }

        //Id 
        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual Group Groups { get; set; }

        public virtual Service Services { get; set; }
    }
}