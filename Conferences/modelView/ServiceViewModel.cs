using System;
using System.Linq;
using Conferences.bdWork;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Conferences.modelView
{
    internal class ServiceViewModel : INotifyPropertyChanged
    {
        private DbContextEntityes _db;

        //Список услуг
        private List<Service> _services;
        public List<Service> Services
        {
            get
            {
                return _services;
            }
            set
            {
                _services = value;
                OnPropertyChanged("Services");
            }
        }

        //Выбранная услуга
        private Service _servicesSelected;
        public Service ServicesSelected
        {
            get
            {
                return _servicesSelected;
            }
            set
            {
                _servicesSelected = value;
                OnPropertyChanged("ServicesSelected");
            }
        }

        //Команда - удалить услугу
        private RelayCommand commandDeleteService;
        public RelayCommand CommandDeleteService
        {
            get
            {
                return commandDeleteService ?? (commandDeleteService = new RelayCommand(obj =>
                {
                    if (ServicesSelected == null)
                        return;

                    //Удаление связей с группами
                    List<GroupService> ids = _db.GroupServices.Where(w => w.IdService == ServicesSelected.IdService).ToList();
                    foreach (var elem in ids)
                        _db.DeleteGroupService(elem);

                    //Удаление связей с организациями
                    List<OrganizationService> ids2 = _db.OrganizationServices.Where(w => w.IdService == ServicesSelected.IdService).ToList();
                    foreach (var elem in ids2)
                        _db.DeleteOrganizationService(elem);

                    _db.DeleteService(ServicesSelected);
                    Services = _db.Services.ToList();
                    ServicesSelected = Services.LastOrDefault();
                    OnPropertyChanged("Services");
                    OnPropertyChanged("ServicesSelected");
                }));
            }
        }

        //Команда - добавить услугу
        private RelayCommand commandAddService;
        public RelayCommand CommandAddService
        {
            get
            {
                return commandAddService ?? (commandAddService = new RelayCommand(obj =>
                {
                    _db.AddService(new Service() { Type = "Неизвестная услуга" });
                    Services = _db.Services.ToList();
                    ServicesSelected = Services.LastOrDefault();
                    OnPropertyChanged("Services");
                    OnPropertyChanged("ServicesSelected");
                }));
            }
        }

        //Команда - сохранить услугу
        private RelayCommand commandSaveService;
        public RelayCommand CommandSaveService
        {
            get
            {
                return commandSaveService ?? (commandSaveService = new RelayCommand(obj =>
                {
                    if (ServicesSelected == null)
                        return;
                    _db.SaveChanges();
                    OnPropertyChanged("Services");
                    OnPropertyChanged("ServicesSelected");
                }));
            }
        }

        //Список поставщиков
        private List<Organization> _organizations;
        public List<Organization> Organizations
        {
            get
            {
                return _organizations;
            }
            set
            {
                _organizations = value;
                OnPropertyChanged("Organizations");
            }
        }

        //Выбранный поставщик
        private Organization _organizationsSelected;
        public Organization OrganizationsSelected
        {
            get
            {
                return _organizationsSelected;
            }
            set
            {
                _organizationsSelected = value;
                ServicesIn = SelectServiceIn();
                ServicesOut = SelectServiceOut();
                OnPropertyChanged("OrganizationsSelected");
            }
        }

        //Команда - добавить поставщика
        private RelayCommand commandAddOrganization;
        public RelayCommand CommandAddOrganization
        {
            get
            {
                return commandAddOrganization ?? (commandAddOrganization = new RelayCommand(obj =>
                {
                    _db.AddOrganization(new Organization()
                    {
                        Name = "Неизвестная организация",
                        DescriptionOrg = "Описание...",
                        Reputation = 0
                    });
                    Organizations = _db.Organizations.ToList();
                    OrganizationsSelected = Organizations.LastOrDefault();
                    OnPropertyChanged("Organizations");
                    OnPropertyChanged("OrganizationsSelected");
                }));
            }
        }

        //Команда - удалить поставщика
        private RelayCommand commandDeleteOrganization;
        public RelayCommand CommandDeleteOrganization
        {
            get
            {
                return commandDeleteOrganization ?? (commandDeleteOrganization = new RelayCommand(obj =>
                {
                    if (OrganizationsSelected == null)
                        return;

                    //Удаление связей с услугами
                    List<OrganizationService> ids = _db.OrganizationServices.Where(w => w.IdOrganization == OrganizationsSelected.IdOrganization).ToList();
                    foreach (var elem in ids)
                        _db.DeleteOrganizationService(elem);

                    _db.DeleteOrganization(OrganizationsSelected);
                    Organizations = _db.Organizations.ToList();
                    OrganizationsSelected = Organizations.LastOrDefault();
                    OnPropertyChanged("Organizations");
                    OnPropertyChanged("OrganizationsSelected");
                }));
            }
        }

        //Команда - сохранить поставщика
        private RelayCommand commandSaveOrganization;
        public RelayCommand CommandSaveOrganization
        {
            get
            {
                return commandSaveOrganization ?? (commandSaveOrganization = new RelayCommand(obj =>
                {
                    if (OrganizationsSelected == null)
                        return;
                    _db.SaveChanges();
                    OnPropertyChanged("Organizations");
                    OnPropertyChanged("OrganizationsSelected");
                }));
            }
        }

        //Список услуг, которые оказывает организация
        private List<OrganizationService> _servicesIn;
        public List<OrganizationService> ServicesIn
        {
            get
            {
                return _servicesIn;
            }
            set
            {
                _servicesIn = value;
                OnPropertyChanged("ServicesIn");
            }
        }

        //Получение списка услуг, которые оказываются организацией
        private List<OrganizationService> SelectServiceIn()
        {
            if (OrganizationsSelected == null)
                return null;
            return _db.OrganizationServices.Where(w => w.IdOrganization == OrganizationsSelected.IdOrganization).ToList();
        }

        //Выделенная услуга среди оказываемых
        private OrganizationService _servicesInSelected;
        public OrganizationService ServicesInSelected
        {
            get
            {
                return _servicesInSelected;
            }
            set
            {
                _servicesInSelected = value;
                OnPropertyChanged("ServicesInSelected");
            }
        }

        //Список услуг, которые не оказывает организация
        private List<Service> _servicesOut;
        public List<Service> ServicesOut
        {
            get
            {
                return _servicesOut;
            }
            set
            {
                _servicesOut = value;
                OnPropertyChanged("ServicesOut");
            }
        }

        //Получение списка услуг, которые не оказываются организацией
        private List<Service> SelectServiceOut()
        {
            if (OrganizationsSelected == null)
                return null;
            var query = _db.OrganizationServices.Where(w => w.IdOrganization == OrganizationsSelected.IdOrganization);
            List<Service> result = new List<Service>();

            foreach(var elem in _db.Services)
            {
                bool b = false;

                foreach (var ids in query)
                    if (elem.IdService == ids.IdService)
                        b = true;
                if (!b)
                    result.Add(elem);
            }
            return result;
        }

        //Выбранная услуга из списка предложенных
        private Service _servicesOutSelected;
        public Service ServicesOutSelected
        {
            get
            {
                return _servicesOutSelected;
            }
            set
            {
                _servicesOutSelected = value;
                if(OrganizationsSelected != null && ServicesOutSelected != null)
                {
                    OrganizationServiceSelected = new OrganizationService()
                    {
                        IdOrganization = OrganizationsSelected.IdOrganization,
                        IdService = ServicesOutSelected.IdService,
                        Coast = 0,
                        Comission = 0,
                        Duration = new DateTime(2017, 01, 01)
                    };
                }
                
                OnPropertyChanged("ServicesOutSelected");
            }
        }

        //Запись таковой услуги, которая будет записана в промежуточной таблице
        private OrganizationService _organizationServiceSelected;
        public OrganizationService OrganizationServiceSelected
        {
            get
            {
                return _organizationServiceSelected;
            }
            set
            {
                _organizationServiceSelected = value;
                OnPropertyChanged("OrganizationServiceSelected");
            }
        }

        //Команда - добавить услугу к оказываемым
        private RelayCommand commandAddOrganizationService;
        public RelayCommand CommandAddOrganizationService
        {
            get
            {
                return commandAddOrganizationService ?? (commandAddOrganizationService = new RelayCommand(obj =>
                {
                    if (OrganizationServiceSelected != null)
                    {
                        _db.AddOrganizationService(OrganizationServiceSelected);
                        ServicesIn = SelectServiceIn();
                        ServicesOut = SelectServiceOut();
                        //OrganizationServiceSelected = null;
                    }
                }));
            }
        }

        //Команда - удалить услугу из оказываемых
        private RelayCommand commandDeleteOrganizationService;
        public RelayCommand CommandDeleteOrganizationService
        {
            get
            {
                return commandDeleteOrganizationService ?? (commandDeleteOrganizationService = new RelayCommand(obj =>
                {
                    if (ServicesInSelected == null)
                        return;
                    var query = _db.OrganizationServices.FirstOrDefault(u => u.IdService == ServicesInSelected.IdService);
                    _db.DeleteOrganizationService(query);
                    ServicesIn = SelectServiceIn();
                    ServicesOut = SelectServiceOut();
                }));
            }
        }

        //Определение событий, на которые будет реагировать интерфейс Вида
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public ServiceViewModel(DbContextEntityes db)
        {
            //Определяем контекст данных и связываем с базой данных
            _db = db;

            //Получаем список услуг и организаций
            Services = _db.Services.ToList();
            Organizations = _db.Organizations.ToList();
        }
    }
}
