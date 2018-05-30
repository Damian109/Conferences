using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Conferences.bdWork
{
    //Класс для хранения строки из таблицы конференций
    [Table("Conference")]
    public class Conference
    {
        //Id конференции
        private int _idConference;

        [Key]
        public int IdConference
        {
            get { return _idConference; }
            set { _idConference = value; }
        }

        //Название конференции
        private string _name;

        [Required]
        [StringLength(80)]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        //Тема конференции
        private string _subject;

        [Required]
        [StringLength(80)]
        public string SubjectConf
        {
            get { return _subject; }
            set { _subject = value; }
        }

        //Дата проведения
        private DateTime _date;

        [Column(TypeName = "DateTime")]
        public DateTime DateConf
        {
            get
            {
                return _date;
            }
            set
            {
               _date = value;
            }
        }

        //Место проведения
        private string _place;

        [Required]
        [StringLength(80)]
        public string Place
        {
            get { return _place; }
            set { _place = value; }
        }

        //Длительность конференции
        private DateTime _duration;

        [Column(TypeName = "DateTime")]
        public DateTime Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }

        //Внешние данные, связь с таблицей ОрганизацияУслуга
        public virtual ICollection<GroupConference> GroupConference { get; set; }

        public Conference()
        {
            GroupConference = new HashSet<GroupConference>();
        }
    }
}