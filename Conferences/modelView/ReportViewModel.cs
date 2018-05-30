using System;
using System.Linq;
using Conferences.bdWork;
using System.Collections.Generic;

namespace Conferences.modelView
{
    internal sealed class ReportViewModel
    {
        private string _report;

        public string Report
        {
            get { return _report; }
            set { _report = value; }
        }

        public ReportViewModel(DbContextEntityes db, DateTime minDate, DateTime maxDate)
        {
            //Начинаем формировать строку отчета
            _report += "Конференции прошедшие c " + minDate.ToShortDateString() + " по " + maxDate.ToShortDateString() + "\r\r";

            //Получаем конференции, которые входят в диапазон
            List<Conference> conf = SelectConferences(db, minDate, maxDate);
            //Для каждой конференции
            foreach(var elem in conf)
            {
                //Выводим название конференции
                _report += "Конференция <" + elem.Name + ">\n";

                //Получаем количество групп
                _report += "Всего групп принимало участие: " + SelectCountGroups(db, elem).ToString() + "\n";

                //Получаем количетво услуг
                _report += "Всего услуг было оказано: " + SelectCountServices(db, elem).ToString() + "\n";

                //Получаем стоимость
                _report += "на сумму: " + SelectCoast(db, elem).ToString() + "\n";

                //Получаем комиссию
                _report += "из них комиссия составила: " + SelectComission(db, elem).ToString() + "\n\n\n";
            }
        }

        public List<Conference> SelectConferences(DbContextEntityes db, DateTime minDate, DateTime maxDate) =>
            db.Conferences.Where(u => u.Duration >= minDate && u.Duration <= maxDate).ToList();

        public int SelectCountGroups(DbContextEntityes db, Conference conf) =>
            db.GroupConferences.Where(u => u.IdConference == conf.IdConference).Count();

        public int SelectCountServices(DbContextEntityes db, Conference conf)
        {
            int result = 0;
            var query = db.GroupConferences.Where(u => u.IdConference == conf.IdConference).Select(s => s.Groups);
            foreach (var elem in db.GroupServices)
            {
                foreach (var ids in query)
                    if (ids.IdGroup == elem.IdGroup)
                        result++;
            }
            return result;
        }

        public double SelectCoast(DbContextEntityes db, Conference conf)
        {
            double result = 0.0d;
            var query = db.GroupConferences.Where(u => u.IdConference == conf.IdConference).Select(s => s.Groups);

            var queryServ = new List<Service>();

            foreach (var elem in db.GroupServices)
            {
                foreach (var ids in query)
                    if (ids.IdGroup == elem.IdGroup)
                        queryServ.Add(elem.Services);
            }

            foreach (var elem in queryServ)
                result += db.OrganizationServices.Where(u => u.IdService == elem.IdService).OrderBy(w => w.Coast).FirstOrDefault().Coast;

            return result;
        }

        public double SelectComission(DbContextEntityes db, Conference conf)
        {
            double result = 0.0d;
            var query = db.GroupConferences.Where(u => u.IdConference == conf.IdConference).Select(s => s.Groups);

            var queryServ = new List<Service>();

            foreach (var elem in db.GroupServices)
            {
                foreach (var ids in query)
                    if (ids.IdGroup == elem.IdGroup)
                        queryServ.Add(elem.Services);
            }

            foreach (var elem in queryServ)
                result += db.OrganizationServices.Where(u => u.IdService == elem.IdService).OrderBy(w => w.Comission).FirstOrDefault().Comission;

            return result;
        }

        //Команда - Сохранить файл
        private RelayCommand commandSave;
        public RelayCommand CommandSave
        {
            get
            {
                return commandSave ?? (commandSave = new RelayCommand(obj =>
                {
                    System.Windows.Forms.SaveFileDialog dialog = new System.Windows.Forms.SaveFileDialog();
                    dialog.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";

                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                        return;
                    // получаем выбранный файл
                    string filename = dialog.FileName;
                    // сохраняем текст в файл
                    System.IO.File.WriteAllText(filename, Report);
                }));
            }
        }
    }
}