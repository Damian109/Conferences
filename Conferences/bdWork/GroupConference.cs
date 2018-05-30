using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Conferences.bdWork
{
    //Класс для хранения строки из таблицы, в которой группы участвуют в конференциях
    [Table("GroupConference")]
    public class GroupConference
    {
        //Группа, внешний ключ
        private int _idGroup;

        public int IdGroup
        {
            get { return _idGroup; }
            set { _idGroup = value; }
        }

        //Конференция, внешний ключ
        private int _idConference;

        public int IdConference
        {
            get { return _idConference; }
            set { _idConference = value; }
        }

        //Id 
        private int _id;
        [Key]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual Conference Conference { get; set; }

        public virtual Group Groups { get; set; }
    }
}