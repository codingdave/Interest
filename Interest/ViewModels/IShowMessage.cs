using System;

namespace Interest.ViewModels
{
    internal interface IShowMessage
    {
        Action<string> ShowMessage { get; set; }
    }
}