using System;
using CodeBase.Infrastructure;

namespace CodeBase.Services.ApplicationRunner
{
    public interface IApplicationRunner : IService
    {
        Action OnApplicationQuit { get; set; }
        void Quit();
        bool IsQuitting();
    }
}