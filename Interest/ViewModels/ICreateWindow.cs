using System;

namespace Interest.ViewModels
{
    public interface ICreateWindow
    {
        public Action OnLoaded { get; set; }
    }
}
