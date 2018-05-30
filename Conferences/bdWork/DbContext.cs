using System.Data.Entity;

namespace Conferences.bdWork
{
    // Наследуем подкласс от DataContext
    // Это будет класс-прослойка между БД и Пользовательским интерфейсом
    public class DbContextEntityes: DbContext
    {
        public DbContextEntityes(string connectionString)
            : base(connectionString) { }

        //Таблицы из базы данных
        //Таблица Конференций
        public virtual DbSet<Conference> Conferences { get; set; }

        //Добавление Конференции
        public void AddConference(Conference conference)
        {
            Conferences.Add(conference);
            SaveChanges();
        }

        //Удаление Конференции
        public void DeleteConference(Conference conference)
        {
            Conferences.Remove(conference);
            SaveChanges();
        }

        //Таблица связи Групп и Конференций
        public virtual DbSet<GroupConference> GroupConferences { get; set; }

        //Добавление новой связи для групп и конференций
        public void AddGroupConference(GroupConference groupConference)
        {
            GroupConferences.Add(groupConference);
            SaveChanges();
        }

        //Удаление связи между группой и конференцией
        public void DeleteGroupConference(GroupConference groupConference)
        {
            GroupConferences.Remove(groupConference);
            SaveChanges();
        }

        //Таблица Групп
        public virtual DbSet<Group> Groups { get; set; }

        //Добавление Группы
        public void AddGroup(Group group)
        {
            Groups.Add(group);
            SaveChanges();
        }

        //Удаление Группы
        public void DeleteGroup(Group group)
        {
            Groups.Remove(group);
            SaveChanges();
        }

        //Таблица представителей
        public virtual DbSet<Representative> Representatives { get; set; }

        //Добавление Представителя
        public void AddRepresentative(Representative representative)
        {
            Representatives.Add(representative);
            SaveChanges();
        }

        //Удаление представителя
        public void DeleteRepresentative(Representative representative)
        {
            Representatives.Remove(representative);
            SaveChanges();
        }

        //Таблица Услуг
        public virtual DbSet<Service> Services { get; set; }

        //Добавление Услуги
        public void AddService(Service service)
        {
            Services.Add(service);
            SaveChanges();
        }

        //Удаление Услуги
        public void DeleteService(Service service)
        {
            Services.Remove(service);
            SaveChanges();
        }

        //Таблица связи Групп и Услуг
        public virtual DbSet<GroupService> GroupServices { get; set; }

        //Добавление новой связи для групп и Услуг
        public void AddGroupService(GroupService groupService)
        {
            GroupServices.Add(groupService);
            SaveChanges();
        }

        //Удаление связи между группой и Услугой
        public void DeleteGroupService(GroupService groupService)
        {
            GroupServices.Remove(groupService);
            SaveChanges();
        }

        //Таблица Организаций
        public virtual DbSet<Organization> Organizations { get; set; }

        //Добавление организации
        public void AddOrganization(Organization organization)
        {
            Organizations.Add(organization);
            SaveChanges();
        }

        //Удаление организации
        public void DeleteOrganization(Organization organization)
        {
            Organizations.Remove(organization);
            SaveChanges();
        }

        //Таблица связи организаций и Услуг
        public virtual DbSet<OrganizationService> OrganizationServices { get; set; }

        //Добавление новой связи для организаций и Услуг
        public void AddOrganizationService(OrganizationService organizationService)
        {
            OrganizationServices.Add(organizationService);
            SaveChanges();
        }

        //Удаление связи между организацией и Услугой
        public void DeleteOrganizationService(OrganizationService organizationService)
        {
            OrganizationServices.Remove(organizationService);
            SaveChanges();
        }
    }
}