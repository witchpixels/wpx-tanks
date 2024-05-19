using Godot;
using Witchpixels.Framework.Diagnostics.Logging;
using Witchpixels.Framework.Diagnostics.Logging.Targets;
using Witchpixels.Framework.Initialization;
using Witchpixels.Framework.Injection;
using Witchpixels.Tanks.Level;

namespace Witchpixels.Tanks;

public partial class Main : Node, IWaitOnReadyNode
{
    public string TargetName => Name;
    public bool IsReady { get; set; }
    public override async void _Ready()
    {
        base._Ready();
        
        var loggingFactory = new LoggerFactory(
            LogLevel.Debug,
            new []
            {
                new GdPrintLogTarget()
            });
        GlobalServiceContainer.RegisterService<ILoggerFactory>(() => loggingFactory);

        var logger = loggingFactory.CreateServiceLogger<Main>();
        logger.Info("BootFlow starting!");

        var levelServiceScene = GD.Load<PackedScene>("res://bootflow/level_service.tscn");
        var levelService = levelServiceScene.Instantiate<LevelService>();
        AddChild(levelService);

        await levelService.WaitOnReady(logger);
        IsReady = true;
        
        var levels = await levelService.ListLevels();
        logger.Info("Levels found: \n {0}", string.Join('\n', levels));
    }
}