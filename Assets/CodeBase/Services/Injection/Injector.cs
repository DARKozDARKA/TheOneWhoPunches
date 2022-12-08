using CodeBase.CameraScripts;
using CodeBase.Characters.Player.Logic;
using CodeBase.Infrastructure.ServiceLocator;
using CodeBase.Services.InputHandler;
using CodeBase.Tools;
using UnityEngine;

namespace CodeBase.Services.Injection
{
    public class Injector : IService
    {
        private readonly AllServices _allServices;

        public Injector(AllServices allServices) => 
            _allServices = allServices;

        public void InjectIntoPlayer(GameObject player, GameObject camera) =>
            player
                .With(_ => _.GetComponent<PlayerMover>()
                    .Construct(camera, _allServices.Single<IInputService>()))
                .With(_ => _.GetComponent<PlayerDash>()
                    .Construct(_allServices.Single<IInputService>()));


        public void InjectIntoCamera(GameObject camera) =>
            camera.GetComponent<CameraFollower>()
                .Construct(_allServices.Single<IInputService>());
    }
}