using System;
using System.Linq;
using Conferences.bdWork;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Conferences.modelView
{
    internal class ConferenceViewModel : INotifyPropertyChanged
    {
        private DbContextEntityes _db;

        //Список конференций, полученный из контекста
        private List<Conference> conferences;
        public List<Conference> Conferences
        {
            get
            {
                return conferences;
            }
            set
            {
                conferences = value;
                OnPropertyChanged("Conferences");
            }
        }

        //Свойство, отражающее количество групп, входящих в конференцию
        private int _countGroups;
        public int CountGroups
        {
            get
            {
                if (ConferenceSelected != null)
                    return _countGroups; 
                return 0;
            }
            private set
            {
                _countGroups = value;
                OnPropertyChanged("CountGroups");
            }
        }

        //Получение количества групп, входящих в конференцию
        public int GetConrerencesCountGroup(int id) => 
            _db.GroupConferences.Where(w => w.IdConference == ConferenceSelected.IdConference).Count();

        //Свойство, отражающее число участников, входящих в конференцию
        private int _countParticipants;
        public int CountParticipants
        {
            get
            {
                if (ConferenceSelected != null)
                    return _countParticipants;
                return 0;
            }
            private set
            {
                _countParticipants = value;
                OnPropertyChanged("CountParticipants");
            }
        }

        //Получение количества групп, входящих в конференцию
        public int GetConferencesCountParticipants(int id)
        {
            IQueryable<GroupConference> tmp = _db.GroupConferences.Where(w => w.IdConference == ConferenceSelected.IdConference);
            int result = 0;
            foreach (var elem in _db.Groups)
                foreach (var ids in tmp)
                    if (elem.IdGroup == ids.IdGroup)
                        result += elem.CountParticipants;
            return result;
        }

        //Выбранная конференция для расширенного просмотра
        private Conference _conferenceSelected;
        public Conference ConferenceSelected
        {
            get { return _conferenceSelected; }
            set
            {
                if (value == null)
                    return;
                _conferenceSelected = value;
                _db.SaveChanges();
                CountGroups = GetConrerencesCountGroup(_conferenceSelected.IdConference);
                CountParticipants = GetConferencesCountParticipants(_conferenceSelected.IdConference);
                Groups = GetConrerencesGroups(_conferenceSelected.IdConference);
                GroupsOut = GetConferencesOutGroup(_conferenceSelected.IdConference);
                OnPropertyChanged("ConferenceSelected");
            }
        }

        //Выбранная  группа из списка групп
        private Group _groupSelected;
        public Group GroupSelected
        {
            get
            {
                return _groupSelected;
            }
            set
            {
                _groupSelected = value;
            }
        }

        //Выбранная  группа из таблицы групп
        private Group _groupTableSelected;
        public Group GroupTableSelected
        {
            get
            {
                return _groupTableSelected;
            }
            set
            {
                _groupTableSelected = value;
            }
        }

        //Список групп, необходим для представления подробной информации
        private List<Group> _groups;
        public List<Group> Groups
        {
            get
            {
                if (ConferenceSelected != null)
                    return _groups;
                return null;
            }
            set
            {
                _groups = value;
                OnPropertyChanged("Groups");
            }
        }

        //Список групп, которые не входят в конференцию.
        private List<Group> _groupsOut;
        public List<Group> GroupsOut
        {
            get
            {
                if (ConferenceSelected != null)
                    return _groupsOut;
                return null;
            }
            set
            {
                _groupsOut = value;
                OnPropertyChanged("GroupsOut");
            }
        }

        //Получение групп, входящих в конференцию
        public List<Group> GetConrerencesGroups(int id)
        {
            IQueryable<GroupConference> tmp = _db.GroupConferences.Where(w => w.IdConference == ConferenceSelected.IdConference);
            List<Group> result = new List<Group>();
            foreach (var elem in _db.Groups)
                foreach (var ids in tmp)
                    if (elem.IdGroup == ids.IdGroup)
                        result.Add(elem);
            return result;
        }

        //Получение групп, не входящих в конференцию
        public List<Group> GetConferencesOutGroup(int id)
        {
            IQueryable<GroupConference> tmp = _db.GroupConferences.Where(w => w.IdConference == ConferenceSelected.IdConference);
            List<Group> result = new List<Group>();
            foreach (var elem in _db.Groups)
            {
                bool b = false;

                foreach (var ids in tmp)
                    if (elem.IdGroup == ids.IdGroup)
                        b = true;
                if(!b)
                    result.Add(elem);
            }
            return result;
        }

        //Определение событий, на которые будет реагировать интерфейс Вида
        //Событие изменения свойства выделенной конференции
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        //Определение команд для элементов управления

        //Команда добавления новой конференции. 
        //Привязывается к кнопке добавить
        private RelayCommand commandButtonAddConference;
        public RelayCommand CommandButtonAddConference
        {
            get
            {
                return commandButtonAddConference ??
                    (commandButtonAddConference = new RelayCommand(obj =>
                    {
                        _db.AddConference(new Conference()
                        {
                            Name = "Конференция",
                            Place = "Адрес",
                            SubjectConf = "Предмет конференции",
                            DateConf = new DateTime(2018, 1, 1),
                            Duration = new DateTime(2018, 1, 2)
                        });
                        Conferences = _db.Conferences.ToList();
                        ConferenceSelected = Conferences.Last();
                        OnPropertyChanged("ConferenceSelected");
                        OnPropertyChanged("Conferences");
                    }));
            }
        }

        //Команда добавления группы в конференцию. 
        //Привязывается к контекстному меню таблицы групп
        private RelayCommand commandButtonAddGroup;
        public RelayCommand CommandButtonAddGroup
        {
            get
            {
                return commandButtonAddGroup ??
                  (commandButtonAddGroup = new RelayCommand(obj =>
                  {
                      if (GroupSelected == null)
                          return;
                      GroupConference tmp = new GroupConference() {
                          IdConference = ConferenceSelected.IdConference, IdGroup = GroupSelected.IdGroup };
                      _db.AddGroupConference(tmp);
                      Groups = GetConrerencesGroups(_conferenceSelected.IdConference);
                      GroupsOut = GetConferencesOutGroup(_conferenceSelected.IdConference);
                      CountGroups = GetConrerencesCountGroup(_conferenceSelected.IdConference);
                      CountParticipants = GetConferencesCountParticipants(_conferenceSelected.IdConference);
                      OnPropertyChanged("Groups");
                  }));
            }
        }

        //Удаление конференций.  По одной
        private RelayCommand commanddeleteConference;
        public RelayCommand CommandDeleteConference
        {
            get
            {
                return commanddeleteConference ??
                  (commanddeleteConference = new RelayCommand(obj =>
                  {
                      if (ConferenceSelected == null)
                          return;

                      //Удаление связей группы с конференциями
                      List<GroupConference> ids = _db.GroupConferences.Where(w => w.IdGroup == ConferenceSelected.IdConference).ToList();
                      foreach (var elem in ids)
                          _db.DeleteGroupConference(elem);

                      _db.DeleteConference(ConferenceSelected);
                      Conferences = _db.Conferences.ToList();
                      ConferenceSelected = Conferences.Last();
                  }));
            }
        }

        //Удаление групп.
        private RelayCommand commandDeleteGroup;
        public RelayCommand CommandDeleteGroup
        {
            get
            {
                return /*commandDeleteGroup ??*/
                    (commandDeleteGroup = new RelayCommand(obj =>
                    {
                        if (GroupTableSelected == null)
                            return;
                        GroupConference tmp = _db.GroupConferences.Where(w => w.IdConference == ConferenceSelected.IdConference &&
                            w.IdGroup == GroupTableSelected.IdGroup).First();
                        _db.DeleteGroupConference(tmp);
                        Groups = GetConrerencesGroups(ConferenceSelected.IdConference);
                        GroupsOut = GetConferencesOutGroup(ConferenceSelected.IdConference);
                        CountGroups = GetConrerencesCountGroup(_conferenceSelected.IdConference);
                        CountParticipants = GetConferencesCountParticipants(_conferenceSelected.IdConference);
                        OnPropertyChanged("ConferenceSelected");
                    }));
            }
        }

        //Переход к редактированию групп
        private RelayCommand commandGroupView;
        public RelayCommand CommandGroupView
        {
            get
            {
                return commandGroupView ?? (commandGroupView = new RelayCommand(obj =>
                {
                    GroupView groupView = new GroupView(_db);
                    groupView.ShowDialog();
                }));
            }
        }

        //Для формирования отчета

        //Минимальная дата
        private DateTime _minDate;
        public DateTime MinDate
        {
            get { return _minDate; }
            set { _minDate = value; }
        }
        //Максимальная дата
        private DateTime _maxDate;
        public DateTime MaxDate
        {
            get { return _maxDate; }
            set { _maxDate = value; }
        }

        //Переход к редактированию групп
        private RelayCommand commandReportView;
        public RelayCommand CommandReportView
        {
            get
            {
                return commandReportView ?? (commandReportView = new RelayCommand(obj =>
                {
                    ReportView reportView = new ReportView(_db, _minDate, _maxDate);
                    reportView.ShowDialog();
                }));
            }
        }

        public ConferenceViewModel()
        {
            //Определяем контекст данных и связываем с базой данных
            _db = new DbContextEntityes("name=DbConnectionString");
            //Получаем список конференций
            Conferences = _db.Conferences.ToList();
            MinDate = Conferences.OrderBy(w => w.Duration).FirstOrDefault().Duration;
            MaxDate = Conferences.OrderByDescending(w => w.Duration).FirstOrDefault().Duration;
        }
    }
}