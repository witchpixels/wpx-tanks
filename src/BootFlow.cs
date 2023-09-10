using System;
using Godot;
using Witchpixels.Tanks.Initialization;
using Witchpixels.Tanks.Level;
using Witchpixels.Tanks.Logging;

namespace Witchpixels.Tanks;

public partial class BootFlow : Node
{
    public override async void _Ready()
    {
        base._Ready();

        var serviceManager = new ServiceManager();
        
        IOC.Registry = serviceManager;
        IOC.DependencyGraph = serviceManager;

        var loggingFactory = new GdPrintLoggerFactory();
        serviceManager.RegisterService<ILoggerFactory>(loggingFactory);

        var logger = loggingFactory.CreateServiceLogger<BootFlow>();
        
        logger.Info("BootFlow starting!");

        var levelService = GetNode<LevelService>("services/LevelService") as ILevelService;

        if (levelService == null)
        {
            logger.Error("Could not find a level service at services/LevelService");
            throw new NullReferenceException("Could not find a level service at services/LevelService");
        }

        await levelService.WaitOnReady(logger);

        var levels = await levelService.ListLevels();
        logger.Info("Levels found: \n {0}", string.Join('\n', levels));
        
        await levelService.LoadLevel("debug/empty_level.tscn");
    }
}