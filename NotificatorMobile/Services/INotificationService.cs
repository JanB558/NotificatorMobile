using NotificatorMobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificatorMobile.Services
{
    internal interface INotificationService
    {
        Task<IEnumerable<Notification>?> GetAll();
        Task<Notification?> GetById(Guid id);
        Task Delete(Guid id);
        Task Create(Notification notification);
        Task Update(Notification notification);
    }
}
