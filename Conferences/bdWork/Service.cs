using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Conferences.bdWork
{
    //Класс для хранения строки из таблицы услуг
    [Table("ServiceType")]
    public class Service
    {
        public Service()
        {
            OrganizationService = new HashSet<OrganizationService>();
            GroupService = new HashSet<GroupService>();
        }

        //Id услуги, задается в БД
        private int _id;

        [Key]
        public int IdService
        {
            get { return _id; }
            set { _id = value; }
        }

        //Тип услуги
        [Required]
        [StringLength(80)]
        public string Type { get; set; }

        //Внешние данные, связь с таблицей ГруппаУслуга
        public virtual ICollection<GroupService> GroupService { get; set; }

        //Внешние данные, связь с таблицей ОрганизацияУслуга
        public virtual ICollection<OrganizationService> OrganizationService { get; set; }
    }
}