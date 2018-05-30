using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Conferences.bdWork
{
    //Класс для хранения строки из таблицы организаций, оказывающих услуги
    [Table("Representative")]
    public class Representative
    {
        //Id представителя, задается в БД
        [Key]
        public int IdRepresentative { get; set; }

        //ФИО представителя
        [Required]
        [StringLength(80)]
        public string Name { get; set; }

        //Количество групп, которые ведет данный представитель
        public int CountGroup { get; set; }

        //Количество конференций, в которых учавствовал или участвует данный представитель
        public int CountConference { get; set; }


        //Внешние данные, связь с таблицей групп
        public virtual ICollection<Group> Groups { get; set; }

        public Representative()
        {
            Groups = new HashSet<Group>();
        }
    }
}