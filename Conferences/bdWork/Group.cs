using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Conferences.bdWork
{
    //Класс для хранения строки из таблицы групп, учавствующих в конференциях
    [Table("Groups")]
    public class Group
    {
        //Id группы, задается в БД
        private int _id;

        [Key]
        public int IdGroup
        {
            get { return _id; }
            set { _id = value; }
        }

        //Представитель группы, внешний ключ
        private int? _idRepresentative;
        public virtual Representative Representative { get; set; }

        public int? IdRepresentative
        {
            get { return _idRepresentative; }
            set { _idRepresentative = value; }
        }

        //Название группы
        private string _name;

        [Required]
        [StringLength(80)]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        //Количество участников
        private int _countParticipants;

        public int CountParticipants
        {
            get { return _countParticipants; }
            set { _countParticipants = value; }
        }

        public virtual ICollection<GroupConference> GroupConference { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public Group()
        {
            GroupConference = new HashSet<GroupConference>();
        }
    }
}