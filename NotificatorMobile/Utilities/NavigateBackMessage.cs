using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificatorMobile.Utilities
{
    public class NavigateBackMessage : ValueChangedMessage<bool>
    {
        public NavigateBackMessage(bool value) : base(value) { }
    }
}
