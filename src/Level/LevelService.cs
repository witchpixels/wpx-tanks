using System.Threading.Tasks;
using Godot;
using Witchpixels.Tanks.Initialization;
using Witchpixels.Tanks.Logging;

namespace Witchpixels.Tanks.Level;

public interface ILevelService
{
    Task LoadLevel(string levelName, string? stageName);
}

public partial class LevelService : Node3D, ILevelService
{
    private ILogger _logger;

    [Export] private Camera3D _stageCamera;
    [Export] private string _stagesPath = "res://environment/stages/";
    [Export] private string _shippedLevelsPath = "res://levels/";
    [Export] private Node3D? _defaultStage;

    private Node3D? _currentLevel;
    private Node3D? _currentStage;

    public override async void _Ready()
    {
        base._Ready();

        await IOC.WaitOnReady();

        IOC.DependencyGraph.Require(Name)
            .DependsOn<ILoggerFactory>(lf => _logger = lf.CreateServiceLogger<LevelService>())
            .WhenReady(() =>
            {
                IOC.Registry.RegisterService<ILevelService>(this);
                _logger.Info("Level service is loaded!");
            });
    }
    
    public async Task LoadLevel(string levelName, string? stageName)
    {
        await UnloadCurrentLevel();
        
        // wpx2023 TODO: Add another check after this to look in user folder so that UGC is allowed
        var levelPath = $"{_shippedLevelsPath}{levelName}";

        if (ResourceLoader.Exists(levelPath))
        {
            _logger.Info("Loading level {0} on stage {1} from path {3}", 
                levelName, 
                stageName ?? "default", 
                levelPath);

            var levelScene = ResourceLoader.Load<PackedScene>(levelPath);
            _currentLevel = levelScene.Instantiate<Node3D>();
            
            _logger.Info("Finished loading level resource");
        }

        var stagePath = $"{_stagesPath}{stageName}";
        if (stageName is not null && ResourceLoader.Exists(stageName))
        {
            _logger.Info("Loading stage {0} from {1}", stageName, stagePath);
            
            
            
            _logger.Info("Loaded stage");
        }
        else if (_defaultStage is not null)
        {
            _defaultStage.Visible = true;
        }
    }

    private async Task UnloadCurrentLevel()
    {
        if (_currentLevel is not null)
        {
            _logger.Info("Unloading current level...");
            _currentLevel.QueueFree();
            await ToSignal(_currentLevel, "tree_Exited");
            
            _logger.Info("Done unloading current level!");
        }

        if (_currentStage is not null && _currentStage != _defaultStage)
        {
            _logger.Info("Unloading current stage...");
            
            _currentStage.QueueFree();
            await ToSignal(_currentStage, "tree_Exited");
            
            _logger.Info("Done unloading current stage!");
        }
    }
}