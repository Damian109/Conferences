using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Conferences.bdWork
{
    //Класс для хранения строки из таблицы организаций, оказывающих услуги
    [Table("Organization")]
    public class Organization
    {
        public Organization()
        {
            OrganizationService = new HashSet<OrganizationService>();
        }

        //Id организации, задается в БД
        private int _id;

        [Key]
        public int IdOrganization
        {
            get { return _id; }
            set { _id = value; }
        }

        //Название организации
        [Required]
        [StringLength(80)]
        public string Name { get; set; }

        //Описание организации
        [Required]
        [StringLength(200)]
        public string DescriptionOrg { get; set; }

        //Репутация организации
        public int Reputation { get; set; }

        //Внешние данные, связь с таблицей ОрганизацияУслуга
        public virtual ICollection<OrganizationService> OrganizationService { get; set; }
    }
}