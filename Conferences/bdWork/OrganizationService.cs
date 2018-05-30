using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Conferences.bdWork
{
    //Класс для хранения строки из таблицы, в которой Услуги предлагаются организациями
    [Table("OrganizationService")]
    public class OrganizationService
    {
        //Услуга, внешний ключ
        public int? IdService { get; set; }

        //Организация, внешний ключ
        public int? IdOrganization { get; set; }

        //Конечная стоимость услуги
        private double _coast;

        public double Coast
        {
            get { return _coast; }
            set { _coast = value; }
        }

        //Время, за которое услуга будет оказана, время ожидания
        private DateTime _duration;

        public DateTime Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }

        //Сколько фирма получит в качестве комиссии
        private double _comission;

        public double Comission
        {
            get { return _comission; }
            set { _comission = value; }
        }

        //Id 
        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual Organization Organization { get; set; }

        public virtual Service ServiceType { get; set; }
    }
}