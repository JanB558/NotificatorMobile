using NotificatorMobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificatorMobile.Services
{
    public interface INotificationService
    {
        Task<IEnumerable<Notification>?> GetAll();
        Task<Notification?> GetById(int id);
        Task Delete(int id);
        Task Create(Notification notification);
        Task Update(Notification notification);
        void Register(Notification notification);
        void Cancel(int id);
    }
}
