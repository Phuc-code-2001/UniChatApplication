using System;

namespace UniChatApplication.Models
{
    public class ErrorViewModel
    {
        //RequestId Property
        public string RequestId { get; set; }
        //Get ShowRequestId
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
