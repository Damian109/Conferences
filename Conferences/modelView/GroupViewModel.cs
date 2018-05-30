using System.Linq;
using Conferences.bdWork;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Conferences.modelView
{
    internal class GroupViewModel : INotifyPropertyChanged
    {
        private DbContextEntityes _db;

        //Список групп, полученный из контекста
        private List<Group> _groups;
        public List<Group> Groups
        {
            get
            {
                return _groups;
            }
            set
            {
                _groups = value;
                OnPropertyChanged("Groups");
            }
        }

        //Команда добавления новой группы. 
        private RelayCommand commandAddGroup;
        public RelayCommand CommandAddGroup
        {
            get
            {
                return commandAddGroup ?? (commandAddGroup = new RelayCommand(obj =>
                {
                    _db.AddGroup(new Group()
                    {
                        Name = "Группа",
                        CountParticipants = 10,
                        IdRepresentative = null
                    });
                    Groups = _db.Groups.ToList();
                    GroupSelected = Groups.Last();
                    OnPropertyChanged("GroupSelected");
                    OnPropertyChanged("Groups");
                }));
            }
        }

        //Команда удаления группы.
        private RelayCommand commandDeleteGroup;
        public RelayCommand CommandDeleteGroup
        {
            get
            {
                return commandDeleteGroup ??(commandDeleteGroup = new RelayCommand(obj =>
                  {
                      if (GroupSelected == null)
                          return;

                      //Удаление связей группы с конференциями
                      List<GroupConference> ids = _db.GroupConferences.Where(w => w.IdGroup == GroupSelected.IdGroup).ToList();
                      foreach (var elem in ids)
                          _db.DeleteGroupConference(elem);

                      //Удаление связей с услугами
                      List<GroupService> ids2 = _db.GroupServices.Where(w => w.IdGroup == GroupSelected.IdGroup).ToList();
                      foreach (var elem in ids2)
                          _db.DeleteGroupService(elem);

                      _db.DeleteGroup(GroupSelected);
                      Groups = _db.Groups.ToList();
                      GroupSelected = Groups.Last();
                      OnPropertyChanged("GroupSelected");
                      OnPropertyChanged("Groups");
                  }));
            }
        }

        //Представитель для отображения
        private Representative _representativeS;
        public Representative RepresentativeS
        {
            get { return _representativeS; }
            set
            {
                if (value == null)
                    return;
                _representativeS = value;
                OnPropertyChanged("RepresentativeS");
            }
        }

        //Получение представителя для отображения
        private Representative SelectRepresentativeS()
        {
            if (GroupSelected != null)
                return _db.Representatives.Where(w => w.IdRepresentative == GroupSelected.IdRepresentative).FirstOrDefault();
            return null;
        }

        //Представители, оставшиеся незатронутыми
        private List<Representative> _representativeListS;
        public List<Representative> RepresentativeListS
        {
            get { return _representativeListS; }
            set
            {
                if (value == null)
                    return;
                _representativeListS = value;
                OnPropertyChanged("RepresentativeListS");
            }
        }

        //Получение представителей, оставшихся незатронутыми
        private List<Representative> SelectRepresentativeListS()
        {
            if (GroupSelected == null)
                return null;
            return _db.Representatives.Where(w => w.IdRepresentative != GroupSelected.IdRepresentative).ToList();
        }

        //Выбранный представитель из списка
        private Representative _representativeSelected;
        public Representative RepresentativeSelected
        {
            get { return _representativeSelected; }
            set
            {
                if (value == null)
                    return;
                _representativeSelected = value;
                OnPropertyChanged("RepresentativeSelected");
            }
        }

        //Команда назначения представителя группе. 
        private RelayCommand commandTakeRepresentative;
        public RelayCommand CommandTakeRepresentative
        {
            get
            {
                return commandTakeRepresentative ?? (commandTakeRepresentative = new RelayCommand(obj =>
                {
                    if (RepresentativeSelected == null)
                        return;
                    _db.Groups.Where(w => w.IdGroup == GroupSelected.IdGroup).FirstOrDefault().IdRepresentative =
                        RepresentativeSelected.IdRepresentative;
                    _db.SaveChanges();
                    RepresentativeS = SelectRepresentativeS();
                    RepresentativeListS = SelectRepresentativeListS();
                    OnPropertyChanged("GroupSelected");
                    OnPropertyChanged("Groups");
                }));
            }
        }

        //Команда добавления нового представителя.
        private RelayCommand commandAddRepresentative;
        public RelayCommand CommandAddRepresentative
        {
            get
            {
                return commandAddRepresentative ?? (commandAddRepresentative = new RelayCommand(obj =>
                {
                    _db.AddRepresentative(new Representative()
                    {
                        Name = "Представитель",
                        CountConference = 0,
                        CountGroup = 0
                    });
                    RepresentativeS = SelectRepresentativeS();
                    RepresentativeListS = SelectRepresentativeListS();
                }));
            }
        }

        //Конференции, в которые входит выбранная группа
        private List<Conference> _conferenceS;
        public List<Conference> ConferenceS
        {
            get { return _conferenceS; }
            set
            {
                if (value == null)
                    return;
                _conferenceS = value;
                OnPropertyChanged("ConferenceS");
            }
        }

        //Получение конференций, в которые входит выбранная группа
        private List<Conference> SelectConferenceS()
        {
            if (GroupSelected == null)
                return null;
            var query = _db.GroupConferences.Where(w => w.IdGroup == GroupSelected.IdGroup);
            List<Conference> result = new List<Conference>();
            foreach (var elem in query)
                result.Add(_db.Conferences.Where(w => w.IdConference == elem.IdConference).FirstOrDefault());
            return result;
        }

        //Список услуг, которые требуются группе
        private List<Service> _serviceIn;
        public List<Service> ServiceIn
        {
            get
            {
                return _serviceIn;
            }
            set
            {
                _serviceIn = value;
                OnPropertyChanged("ServiceIn");
            }
        }

        //Получение услуг, требующихся группе
        private List<Service> SelectServiceIn()
        {
            if (GroupSelected == null)
                return null;
            var query = _db.GroupServices.Where(w => w.IdGroup == GroupSelected.IdGroup);

            List<Service> result = new List<Service>();
            foreach (var elem in query)
                result.Add(_db.Services.Where(w => w.IdService == elem.IdService).FirstOrDefault());
            return result;
        }

        //Услуга, которая выделена в таблице
        private Service _serviceInSelect;
        public Service ServiceInSelect
        {
            get { return _serviceInSelect; }
            set
            {
                _serviceInSelect = value;
                Coast = SelectCoast();
                OrganizationName = SelectOrganizationName();
                OnPropertyChanged("ServiceinSelect");
            }
        }

        //Список услуг, которые еще не требуются группе
        private List<Service> _serviceOut;
        public List<Service> ServiceOut
        {
            get
            {
                return _serviceOut;
            }
            set
            {
                _serviceOut = value;
                OnPropertyChanged("ServiceOut");
            }
        }

        //Получение услуг, еще не требующихся группе
        private List<Service> SelectServiceOut()
        {
            if (GroupSelected == null)
                return null;
            IQueryable<GroupService> query = _db.GroupServices.Where(w => w.IdGroup == GroupSelected.IdGroup);
            List<Service> result = new List<Service>();

            foreach(Service elem in _db.Services)
            {
                bool b = false;

                foreach (GroupService ids in query)
                    if (elem.IdService == ids.IdService)
                        b = true;
                if(!b)
                    result.Add(elem);
            }
            return result;
        }

        //Услуга, которая выбрана в списке предлагаемых
        private Service _serviceOutSelected;
        public Service ServiceOutSelected
        {
            get
            {
                return _serviceOutSelected;
            }
            set
            {
                _serviceOutSelected = value;
                OnPropertyChanged("ServiceOutselected");
            }
        }

        //Команда удаления услуги из таблицы
        private RelayCommand commandDeleteFromServiceIn;
        public RelayCommand CommandDeleteFromServiceIn
        {
            get
            {
                return commandDeleteFromServiceIn ?? (commandDeleteFromServiceIn = new RelayCommand(obj =>
                    {
                        if (ServiceInSelect == null)
                            return;
                        GroupService tmp = _db.GroupServices.FirstOrDefault(u => u.IdService == ServiceInSelect.IdService);
                        _db.DeleteGroupService(tmp);
                        ServiceIn = SelectServiceIn();
                        ServiceOut = SelectServiceOut();
                        ServiceInSelect = ServiceIn.FirstOrDefault();
                        OnPropertyChanged("ServiceIn");
                    }));
            }
        }
        
        //Команда добавления выбранной услуги
        private RelayCommand commandButtonAddServiceIn;
        public RelayCommand CommandButtonAddServiceIn
        {
            get
            {
                return commandButtonAddServiceIn ?? (commandButtonAddServiceIn = new RelayCommand(obj =>
                  {
                      if (ServiceOutSelected == null)
                          return;
                      GroupService tmp = new GroupService()
                      {
                          IdService = ServiceOutSelected.IdService,
                          IdGroup = GroupSelected.IdGroup
                      };
                      _db.AddGroupService(tmp);
                      ServiceIn = SelectServiceIn();
                      ServiceOut = SelectServiceOut();
                      ServiceInSelect = ServiceIn.FirstOrDefault();
                      OnPropertyChanged("ServiceIn");
                  }));
            }
        }

        //Стоимость услуги
        private string _coast;
        public string Coast
        {
            get
            {
                return _coast;
            }
            set
            {
                _coast = value;
                OnPropertyChanged("Coast");
            }
        }

        //Получение стоимости услуги
        public string SelectCoast()
        {
            if (ServiceInSelect == null)
                return "";
            OrganizationService query = _db.OrganizationServices.Where(w => w.IdService == 
                ServiceInSelect.IdService).OrderBy(w => w.Coast).FirstOrDefault();
            return query == null ? "" : query.Coast.ToString();
        }

        //Организация, оказывающая выбранную услугу
        private string _organizationName;
        public string OrganizationName
        {
            get
            {
                return _organizationName;
            }
            set
            {
                _organizationName = value;
                OnPropertyChanged("OrganizationName");
            }
        }

        //Получение названия этой организации
        public string SelectOrganizationName()
        {
            if (ServiceInSelect == null)
                return "";
            OrganizationService query = _db.OrganizationServices.Where(w => w.IdService ==
                ServiceInSelect.IdService).OrderBy(w => w.Coast).FirstOrDefault();
            return query == null ? "" : query.Organization.Name;
        }

        //Команда перехода к меню редактирования услуг
        private RelayCommand commandServiceView;
        public RelayCommand CommandServiceView
        {
            get
            {
                return commandServiceView ?? (commandServiceView = new RelayCommand(obj =>
                {
                    ServiceView serviceView = new ServiceView(_db);
                    serviceView.ShowDialog();
                }));
            }
        }

        //Выбранная конференция для расширенного просмотра
        private Group _groupSelected;
        public Group GroupSelected
        {
            get { return _groupSelected; }
            set
            {
                if (value == null)
                    return;
                _groupSelected = value;
                _db.SaveChanges();
                RepresentativeS = SelectRepresentativeS();
                RepresentativeListS = SelectRepresentativeListS();
                ConferenceS = SelectConferenceS();
                ServiceIn = SelectServiceIn();
                ServiceOut = SelectServiceOut();
                ServiceInSelect = ServiceIn.FirstOrDefault();
                OnPropertyChanged("GroupSelected");
            }
        }

        //Определение событий, на которые будет реагировать интерфейс Вида
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public GroupViewModel(DbContextEntityes db)
        {
            //Определяем контекст данных и связываем с базой данных
            _db = db;

            //Получаем список групп
            Groups = _db.Groups.ToList();
        }
    }
}