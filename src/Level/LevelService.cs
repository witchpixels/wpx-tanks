using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using Witchpixels.Tanks.Initialization;
using Witchpixels.Tanks.Logging;

namespace Witchpixels.Tanks.Level;

public interface ILevelService : IService
{
    Task LoadLevel(string levelName, string stageName = null);
    Task<IReadOnlyList<string>> ListLevels();
}

public partial class LevelService : Node3D, ILevelService
{
    private ILogger _logger;

    [Export] private Camera3D _stageCamera;
    [Export] private string _stagesPath = "res://environment/stages/";
    [Export] private string _shippedLevelsPath = "res://levels/";
    [Export] private Node3D _defaultStage;

    private Node3D _currentLevel;
    private Node3D _currentStage;

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
                IsReady = true;
            });
    }
    
    public async Task LoadLevel(string levelName, string stageName)
    {
        GetTree().Paused = true;
        await UnloadCurrentLevel();
        
        // wpx2023 TODO: Add another check after this to look in user folder so that UGC is allowed
        var levelPath = $"{_shippedLevelsPath}{levelName}";

        if (ResourceLoader.Exists(levelPath))
        {
            _logger.Info("Loading level {0} on stage {1} from path {2}", 
                levelName, 
                stageName ?? "default", 
                levelPath);

            var levelScene = ResourceLoader.Load<PackedScene>(levelPath);
            _currentLevel = levelScene.Instantiate<Node3D>();
            var readyTask = ToSignal(_currentLevel, "ready"); 
            AddChild(_currentLevel);
            await readyTask;
            
            _logger.Info("Finished loading level resource");
        }
        else
        {
            _logger.Error("Attempted to load level {0}, but it was not found!", levelPath);
        }

        var stagePath = $"{_stagesPath}{stageName}";
        if (stageName is not null && ResourceLoader.Exists(stageName))
        {
            _logger.Info("Loading stage {0} from {1}", stageName, stagePath);

            if (_defaultStage is not null) _defaultStage.Visible = false;

            var stageScene = ResourceLoader.Load<PackedScene>(stagePath);

            _currentStage = stageScene.Instantiate<Node3D>();
            var readyTask = ToSignal(_currentStage, "ready"); 
            AddChild(_currentStage);
            await readyTask;
            
            _logger.Info("Loaded stage");
        }
        else if (_defaultStage is not null)
        {
            _defaultStage.Visible = true;
        }

        GetTree().Paused = false;
        _logger.Info("Level loaded, unpaused");
    }

    public Task<IReadOnlyList<string>> ListLevels()
    {
        var levels = new List<string>();
        
        var directories = DirAccess.GetDirectoriesAt(_shippedLevelsPath);
        foreach (var directory in directories)
        {
            if (directory is null) continue;
            
            var files = DirAccess.GetFilesAt($"{_shippedLevelsPath}/{directory}");
            
            if (files is null || !files.Any()) continue;
            
            levels.AddRange(files.Select(f => $"{directory}/{f}"));
        }

        return Task.FromResult(levels as IReadOnlyList<string>);
    }

    private async Task UnloadCurrentLevel()
    {
        if (_currentLevel is not null)
        {
            _logger.Info("Unloading current level...");
            
            RemoveChild(_currentLevel);
            _currentLevel.QueueFree();
            await ToSignal(_currentLevel, "tree_Exited");
            
            _logger.Info("Done unloading current level!");
        }

        if (_currentStage is not null && _currentStage != _defaultStage)
        {
            _logger.Info("Unloading current stage...");
            
            RemoveChild(_currentStage);
            _currentStage.QueueFree();
            await ToSignal(_currentStage, "tree_Exited");
            
            _logger.Info("Done unloading current stage!");
        }
    }

    public string ServiceName => Name;
    public bool IsReady { get; set; }
}