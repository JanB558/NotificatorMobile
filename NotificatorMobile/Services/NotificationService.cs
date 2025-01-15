using LiteDB;
using NotificatorMobile.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificatorMobile.Services
{
    internal class NotificationService : INotificationService
    {
        private readonly LiteDatabase _database;
        private readonly ILiteCollection<Notification> _notificationCollection;
        public NotificationService(string databasePath, string collectionName)
        {
            _database = new LiteDatabase(databasePath);
            _notificationCollection = _database.GetCollection<Notification>(collectionName);
        }
        public async Task Create(Notification notification)
        {
            try
            {
                await Task.Run(() => _notificationCollection.Insert(notification));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Database operation failed - {ex}");
            }
        }

        public async Task Delete(Guid id)
        {
            try
            {
                await Task.Run(() => _notificationCollection.Delete(id));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Database operation failed - {ex}");
            }
        }

        public async Task<IEnumerable<Notification>?> GetAll()
        {
            try
            {
                return await Task.Run(() => _notificationCollection.Query().ToList());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Database operation failed - {ex}");
                return null;
            }
        }

        public async Task<Notification?> GetById(Guid id)
        {
            try
            {
                return await Task.Run(() => _notificationCollection.FindById(id));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Database operation failed - {ex}");
                return null;
            }
        }

        public async Task Update(Notification notification)
        {
            try
            {
                await Task.Run(() => _notificationCollection.Update(notification));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Database operation failed - {ex}");
            }
        }
    }
}
