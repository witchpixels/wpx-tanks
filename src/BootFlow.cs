using Godot;
using Witchpixels.Tanks.Initialization;
using Witchpixels.Tanks.Logging;

namespace Witchpixels.Tanks;

public partial class BootFlow : Node
{
    public override void _Ready()
    {
        base._Ready();

        var serviceManager = new ServiceManager();
        
        IOC.Registry = serviceManager;
        IOC.DependencyGraph = serviceManager;

        var loggingFactory = new GdPrintLoggerFactory();
        serviceManager.RegisterService<ILoggerFactory>(loggingFactory);

        var logger = loggingFactory.CreateServiceLogger<BootFlow>();
        
        logger.Info("BootFlow starting!");
    }
}