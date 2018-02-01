///-----------------------------------------------------------------
/// <summary>
/// Communication with Microsoft EWS (Exchange Web Service).
/// </summary>
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Exchange.WebServices.Data;

namespace LibExchange
{
    public class Exchange : IExchange
    {
        private readonly string username = "mwestover@ptc.com";
        private readonly string password = "Googler!37";
        private readonly string exchangeUrl = "https://outlook.office365.com/ews/exchange.asmx";
        private readonly ExchangeVersion exchangeVersion = ExchangeVersion.Exchange2010;

        public List<ExchangeEvent> LoadAppointments(DateTime start, DateTime end, string accountName = null)
        {
            var calendar = GetCalendar(accountName);
            var appointments = GetAppointments(calendar, start, end);
            return ConvertAppointmentsToEvents(appointments);
        }

        private ExchangeService Service
        {
            get
            {
                var service = new ExchangeService(exchangeVersion)
                {
                    Credentials = new WebCredentials(username, password),
                    Url = new Uri(exchangeUrl)
                };
                return service;
            }
        }

        private FindItemsResults<Appointment> GetAppointments(CalendarFolder calendar, DateTime startDate, DateTime endDate)
        {
            var cView = new CalendarView(startDate, endDate, 50)
            {
                PropertySet = new PropertySet(AppointmentSchema.Subject, AppointmentSchema.Start, AppointmentSchema.End, AppointmentSchema.Id)
            };

            FindItemsResults<Appointment> appointments = null;
            System.Threading.Tasks.Task tAppointments = System.Threading.Tasks.Task.Run(async () =>
            {
                appointments = await calendar.FindAppointments(cView);
            });
            tAppointments.Wait();

            return appointments;
        }

        private CalendarFolder GetCalendar(string folderName)
        {
            CalendarFolder calendar = null;
            System.Threading.Tasks.Task tCalendar = System.Threading.Tasks.Task.Run(async () =>
            {
                calendar =
                    string.IsNullOrEmpty(folderName) ?
                    await FindDefaultCalendarFolder() :
                    FindNamedCalendarFolder(folderName);
            });
            tCalendar.Wait();
            return calendar;
        }

        private async Task<CalendarFolder> FindDefaultCalendarFolder() => await CalendarFolder.Bind(Service, WellKnownFolderName.Calendar, new PropertySet());

        private CalendarFolder FindNamedCalendarFolder(string name)
        {
            const int pageSize = 100;
            var view = new FolderView(pageSize)
            {
                PropertySet = new PropertySet(BasePropertySet.IdOnly)
            };
            view.PropertySet.Add(FolderSchema.DisplayName);
            view.Traversal = FolderTraversal.Deep;

            SearchFilter sfSearchFilter = new SearchFilter.IsEqualTo(FolderSchema.FolderClass, "IPF.Appointment");

            FindFoldersResults findFolderResults = null;
            System.Threading.Tasks.Task tFolders = System.Threading.Tasks.Task.Run(async () =>
            {
                findFolderResults = await Service.FindFolders(WellKnownFolderName.Root, sfSearchFilter, view);
            });
            tFolders.Wait();

            var foldersMatchingName = findFolderResults.Where(f => f.DisplayName == name).Cast<CalendarFolder>().FirstOrDefault();
            return foldersMatchingName;
        }

        private List<ExchangeEvent> ConvertAppointmentsToEvents(FindItemsResults<Appointment> appointments)
        {
            var events = new List<ExchangeEvent>();
            foreach (var item in appointments)
            {
                events.Add(new ExchangeEvent
                {
                    Id = item.Id.UniqueId,
                    Subject = item.Subject,
                    Start = item.Start,
                    End = item.End
                });
            }

            return events;
        }
    }
}